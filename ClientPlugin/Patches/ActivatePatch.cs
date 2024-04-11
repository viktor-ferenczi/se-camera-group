using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using HarmonyLib;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.Entities.Blocks;
using VRage.Game.ModAPI.Interfaces;
using VRage.Utils;
using IMyControllableEntity = Sandbox.Game.Entities.IMyControllableEntity;

namespace ClientPlugin
{
    [HarmonyPatch]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public static class ActivatePatch
    {
        private static MethodBase TargetMethod()
        {
            var cls = AccessTools.TypeByName("Sandbox.Game.Screens.Helpers.MyToolbarItemTerminalGroup");
            if (cls == null)
            {
                MyLog.Default.Error($"{Plugin.Name}: Cannot find class MyToolbarItemTerminalGroup");
                return null;
            }

            var method = AccessTools.Method(cls, "Activate");
            if (method == null)
            {
                MyLog.Default.Error($"{Plugin.Name}: Cannot find method MyToolbarItemTerminalGroup.Activate");
                return null;
            }

            return method;
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> ActivateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var il = instructions.ToList();
            il.RecordOriginalCode();

            // Safety check
            var hash = il.GetCodeHash();
            if (hash != "607700dc")
            {
                MyLog.Default.Warning($"{Plugin.Name}: The code inside MyToolbarItemTerminalGroup.Activate method has changed, not patching it");
                return il;
            }

            // Entirely remove the second foreach loop, except of loading the list variable:
            // foreach (MyTerminalBlock block in myTerminalBlockList)
            var i = il.FindLastIndex(c => c.opcode == OpCodes.Ldloc_3) + 1;
            var j = il.FindLastIndex(c => c.opcode == OpCodes.Ldc_I4_1);
            il.RemoveRange(i, j - i);

            // Inject a static method call instead
            var applyAction = AccessTools.DeclaredMethod(typeof(ActivatePatch), nameof(ApplyAction));
            il.Insert(i++, new CodeInstruction(OpCodes.Ldloc_2));
            il.Insert(i, new CodeInstruction(OpCodes.Call, applyAction));

            il.RecordPatchedCode();
            return il;
        }

        public static void ApplyAction(List<MyTerminalBlock> terminalBlocks, ITerminalAction action)
        {
            // Is it a relevant action?
            // All blocks in the group has a camera controller?
            if (MySession.Static != null &&
                (action.Id == "View" || action.Id == "Control") &&
                terminalBlocks.Count != 0 &&
                terminalBlocks.All(IsBlockWithCamera))
            {
                SelectNextCamera(terminalBlocks, action);
                return;
            }

            // Fall back to the original implementation from MyToolbarItemTerminalGroup.Activate
            foreach (var block in terminalBlocks)
            {
                if (block != null && block.IsFunctional)
                    action.Apply(block);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsBlockWithCamera(MyTerminalBlock terminalBlock)
        {
            switch (terminalBlock)
            {
                case MyCameraBlock _:
                case MyLargeTurretBase _:
                case MySearchlight _:
                case MyTurretControlBlock _:
                    return true;

                default:
                    return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IMyCameraController GetBlockCameraController(MyTerminalBlock terminalBlock)
        {
            switch (terminalBlock)
            {
                case MyCameraBlock cameraBlock:
                    return cameraBlock;

                case MyLargeTurretBase turretBase:
                    return turretBase;

                case MySearchlight searchlight:
                    return searchlight;

                case MyTurretControlBlock turretControlBlock:
                    return turretControlBlock.GetCamera();

                default:
                    return null;
            }
        }

        // Records the last activated camera and the corresponding block.
        // This is required in the cases when the plugin needs to exit from
        // the previous camera (stop using it) before being able to select
        // the next one.
        private static IMyCameraController lastActivatedCamera;
        private static MyTerminalBlock lastActivatedBlock;

        private static void SelectNextCamera(List<MyTerminalBlock> terminalBlocks, ITerminalAction action)
        {
            // Sort the blocks by their name which the player can change
            terminalBlocks.Sort((a, b) => a.CustomName.CompareTo(b.CustomName));

            // Get the camera controllers (may be null for turret controllers if the camera does not exist)
            var cameras = terminalBlocks.Select(GetBlockCameraController).ToList();

            // The currently selected camera block or null if no camera is active
            var current = MySession.Static.CameraController;
            var index = current == null ? -1 : cameras.FindIndex(c => c == current);

            // Select the next working camera
            for (var attempt = 0; attempt < cameras.Count; attempt++)
            {
                // Next block with wrap around
                index = (index + 1) % cameras.Count;

                // It must have a camera
                var camera = cameras[index];
                if (camera == null)
                {
                    continue;
                }

                // It must be operational
                var terminalBlock = terminalBlocks[index];
                if (!terminalBlock.IsWorking)
                {
                    continue;
                }

#if DEBUG
                MyAPIGateway.Utilities.ShowNotification($"Selecting {index}: {terminalBlock.CustomName}");
#endif

                // Leave the last activated camera first, this is required for all blocks except of MyCameraBlock
                if (lastActivatedCamera == current && lastActivatedBlock is IMyControllableEntity controllableEntity)
                {
                    controllableEntity.Use();
                }

                // No camera is selected for use (player is back in the cockpit's view)
                lastActivatedCamera = null;
                lastActivatedBlock = null;

                // View the camera or Control the block
                action.Apply(terminalBlock);

                // Check whether the camera has changed as expected
                if (MySession.Static.CameraController == camera)
                {
                    // Record the active camera
                    lastActivatedCamera = camera;
                    lastActivatedBlock = terminalBlock;

                    // Notify the player
                    MyAPIGateway.Utilities.ShowNotification($"{terminalBlock.CustomName}", 1000);
                }

                break;
            }
        }
    }
}