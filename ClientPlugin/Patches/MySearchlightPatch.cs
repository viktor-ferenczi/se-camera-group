using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Sandbox.Game.Gui;
using SpaceEngineers.Game.Entities.Blocks;

namespace ClientPlugin.Patches
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [HarmonyPatch(typeof(MySearchlight))]
    public class MySearchlightPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("CreateTerminalControls")]
        public static void CreateTerminalControlsPostfix()
        {
            var terminalActions = new List<ITerminalAction>();
            MyTerminalControlFactory.GetActions(typeof(MySearchlight), terminalActions);
            foreach (var action in terminalActions)
            {
                if (action is MyTerminalAction<MySearchlight> a && a.Id == "Control")
                {
                    // Enable this action for groups of blocks
                    a.ValidForGroups = true;
                }
            }
        }
    }
}