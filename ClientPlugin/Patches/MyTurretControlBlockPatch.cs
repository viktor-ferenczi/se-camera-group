using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Sandbox.Game.Gui;
using SpaceEngineers.Game.Entities.Blocks;

namespace ClientPlugin.Patches
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [HarmonyPatch(typeof(MyTurretControlBlock))]
    public class MyTurretControlBlockPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("CreateTerminalControls")]
        public static void CreateTerminalControlsPostfix()
        {
            var terminalActions = new List<ITerminalAction>();
            MyTerminalControlFactory.GetActions(typeof(MyTurretControlBlock), terminalActions);
            foreach (var action in terminalActions)
            {
                if (action is MyTerminalAction<MyTurretControlBlock> a && a.Id == "Control")
                {
                    // Enable this action for groups of blocks
                    a.ValidForGroups = true;
                }
            }
        }
    }
}