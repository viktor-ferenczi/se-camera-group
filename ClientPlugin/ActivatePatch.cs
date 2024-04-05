using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Sandbox.Game.Entities.Cube;
using VRage.Utils;

namespace ClientPlugin
{
    [HarmonyPatch]
    [HarmonyDebug]
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
            il.Insert(i, new CodeInstruction(OpCodes.Call, handleCameraGroupMethodInfo));

            il.RecordPatchedCode();
            return il;
        }

        public static void HandleCameraGroup(List<MyTerminalBlock> terminalBlocks)
        {
            throw new Exception("HELL");
            MyLog.Default.Info($"{Plugin.Name}: HandleCameraGroup()");
            // return true;
        }
    }
}