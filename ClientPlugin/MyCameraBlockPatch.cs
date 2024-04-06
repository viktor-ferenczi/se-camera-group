using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;

namespace ClientPlugin
{
    [HarmonyPatch(typeof(MyCameraBlock))]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class MyCameraBlockPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("CreateTerminalControls")]
        public static void CreateTerminalControlsPostfix()
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