using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using VRage.Utils;

namespace ClientPlugin
{
    [HarmonyPatch]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class MyCameraBlockPatch
    {
        private static MethodBase TargetMethod()
        {
            var cls = typeof(MyCameraBlock);

            var method = AccessTools.Method(cls, "CreateTerminalControls");
            if (method == null)
            {
                MyLog.Default.Error($"{Plugin.Name}: Cannot find method MyCameraBlock.CreateTerminalControls");
                return null;
            }

            return method;
        }

        public static void Postfix()
        {
            var terminalActions = new List<ITerminalAction>();
            MyTerminalControlFactory.GetActions(typeof(MyCameraBlock), terminalActions);
            foreach (var action in terminalActions)
            {
                if (action is MyTerminalAction<MyCameraBlock> a && a.Id == "View")
                {
                    a.ValidForGroups = true;
                }
            }
        }
    }
}