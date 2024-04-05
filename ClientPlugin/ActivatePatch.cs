using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using HarmonyLib;
using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using VRage.Utils;

namespace ClientPlugin
{
#if DEBUG
    [HarmonyDebug]
#endif
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
            var handleCameraGroupMethodInfo = AccessTools.DeclaredMethod(typeof(ActivatePatch), nameof(HandleCameraGroup));
            il.Insert(i++, new CodeInstruction(OpCodes.Ldloc_2));
            il.Insert(i, new CodeInstruction(OpCodes.Call, handleCameraGroupMethodInfo));

            il.RecordPatchedCode();
            return il;
        }

        public static void HandleCameraGroup(List<MyTerminalBlock> terminalBlocks, ITerminalAction action)
        {
            // All cameras?
            if (terminalBlocks.Count != 0 && terminalBlocks.All(block => block is IMyCameraBlock))
            {
                SelectNextCamera(terminalBlocks, action);
                return;
            }

            // Original implementation from MyToolbarItemTerminalGroup.Activate
            foreach (var block in terminalBlocks)
            {
                if (block != null && block.IsFunctional)
                    action.Apply(block);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SelectNextCamera(List<MyTerminalBlock> cameras, ITerminalAction action)
        {
            // Sort the cameras by their name which the player can change
            cameras.Sort((a, b) => a.CustomName.CompareTo(b.CustomName));

            // Camera system
            var cameraSystem = cameras[0].CubeGrid.GridSystems.CameraSystem;

            // The currently selected camera block or null if no camera is active
            var currentCamera = cameraSystem.CurrentCamera;
            var nextCameraIndex = currentCamera == null ? 0 : cameras.FindIndex(c => c == currentCamera) + 1;

            // Select the next working camera
            for (var i = 0; i < cameras.Count; i++)
            {
                nextCameraIndex %= cameras.Count;
                var camera = cameras[nextCameraIndex];

                if (camera.IsWorking)
                {
                    action.Apply(camera);
                    if (cameraSystem.CurrentCamera == camera)
                    {
                        break;
                    }
                }

                nextCameraIndex++;
            }
        }
    }
}