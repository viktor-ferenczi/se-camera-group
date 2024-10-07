using System;
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
using SpaceEngineers.Game.Entities.Blocks;
using VRage.Game.ModAPI.Interfaces;
using VRage.Utils;
using IMyControllableEntity = Sandbox.Game.Entities.IMyControllableEntity;
using ITerminalAction = Sandbox.ModAPI.Interfaces.ITerminalAction;

namespace ClientPlugin.Patches
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
        private static IEnumerable<CodeInstruction> ActivateTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator gen)
        {
            var il = instructions.ToList();
            il.RecordOriginalCode();

            // Safety check
            var actual = il.GetCodeHash();
            const string expected = "f1c66542";
            if (actual != expected)
            {
                MyLog.Default.Warning($"{Plugin.Name}: The code inside MyToolbarItemTerminalGroup.Activate method has changed, not patching it. Expected hash: {expected}, actual hash: {actual}");
                
                if (Environment.GetEnvironmentVariable("SE_PLUGIN_THROW_ON_FAILED_METHOD_VERIFICATION") != null)
                {
                    throw new Exception("The code inside MyToolbarItemTerminalGroup.Activate method has changed");
                }
                
                return il;
            }

            // Static method to call to handle the view actions
            var handleViewAction = AccessTools.DeclaredMethod(typeof(ActivatePatch), nameof(HandleViewAction));

            // New label to skip the rest of the method, points to the "return true" statement
            var skip = gen.DefineLabel();
            il[il.Count - 2].labels.Add(skip);

            // Inject a static method call after the first for loop which builds the list of actions.
            // If the method returns true, then skip the rest of the method and return true.
            // IMPORTANT: The original Ldloc_3 is kept in place and reused, because it has labels on it.
            //            A new Ldloc_3 is inserted after the conditional jump.
            var i = il.FindLastIndex(c => c.opcode == OpCodes.Ldloc_3) + 1;
            il.Insert(i++, new CodeInstruction(OpCodes.Ldloc_2));
            il.Insert(i++, new CodeInstruction(OpCodes.Call, handleViewAction));
            il.Insert(i++, new CodeInstruction(OpCodes.Brtrue, skip));
            il.Insert(i, new CodeInstruction(OpCodes.Ldloc_3));

            il.RecordPatchedCode();
            return il;
        }

        private static bool HandleViewAction(List<MyTerminalBlock> terminalBlocks, List<ITerminalAction> actions)
        {
#if DEBUG
            MyLog.Default.Debug($"terminalBlocks.Count={terminalBlocks.Count}");
            MyLog.Default.Debug($"actions.Count={actions.Count}");
#endif

            // Keep only the first View or Control action
            var action = actions.FirstOrDefault(a => a.Id == "View" || a.Id == "Control");
#if DEBUG
            MyLog.Default.Debug($"action.Id={action?.Id ?? "null"}");
#endif

            // All blocks in the group has a camera controller?
            if (action != null &&
                MySession.Static != null &&
                terminalBlocks.Any())
            {
                var blocks = terminalBlocks.Where(b => b != null && b.IsFunctional && IsBlockWithCamera(b)).ToList();
                if (blocks.Any())
                {
                    SelectNextCamera(blocks, action);
                    return true;
                }
            }

            // Fall back to the original implementation
            return false;
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

                    // Show camera info on next update (here it is not reliable)
                    Plugin.ShowCameraInfoLater(terminalBlock.CubeGrid.DisplayName, terminalBlock.CustomName.ToString());

                    // Notify the player
                    MyAPIGateway.Utilities.ShowNotification($"{terminalBlock.CustomName}", 500);
                }

                break;
            }
        }
    }
}