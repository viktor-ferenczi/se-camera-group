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
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public static class ActivatePatch
    {
        private static MethodBase TargetMethod()
        {
            var cls = Type.GetType("Sandbox.Game.Screens.Helpers.MyToolbarItemTerminalGroup, Sandbox.Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            if (cls == null)
            {
                MyLog.Default.Error($"{Plugin.Name}: Cannot find class MyToolbarItemTerminalGroup");
                return null;
            }

            var method = AccessTools.GetDeclaredMethods(cls).FirstOrDefault(m => m.Name == "Activate");
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

            // Find the last foreach loop:
            // foreach (MyTerminalBlock block in myTerminalBlockList)
            var i = il.FindLastIndex(c => c.opcode == OpCodes.Ldloc_3);
            var skip = il[i].labels[0];

            // Insert call to static method, return with true if the static method returns true
            il.Insert(i++, new CodeInstruction(OpCodes.Ldloc_3));
            il.Insert(i++, new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(ActivatePatch), nameof(HandleCameraGroup))));
            il.Insert(i++, new CodeInstruction(OpCodes.Brfalse, operand: skip));
            il.Insert(i++, new CodeInstruction(OpCodes.Ldc_I4_1));
            il.Insert(i, new CodeInstruction(OpCodes.Ret));

            il.RecordPatchedCode();
            return il;
        }

        public static bool HandleCameraGroup(List<MyTerminalBlock> terminalBlocks)
        {
            MyLog.Default.Info($"{Plugin.Name}: HandleCameraGroup()");
            return true;
        }
    }
}