using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Sandbox.Game.Gui;
using Sandbox.Game.Weapons;

namespace ClientPlugin
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [HarmonyPatch(typeof(MyLargeTurretBase))]
    public class MyLargeTurretBasePatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("CreateTerminalControls")]
        public static void CreateTerminalControlsPostfix()
        {
            var terminalActions = new List<ITerminalAction>();
            MyTerminalControlFactory.GetActions(typeof(MyLargeTurretBase), terminalActions);
            foreach (var action in terminalActions)
            {
                if (action is MyTerminalAction<MyLargeTurretBase> a && a.Id == "Control")
                {
                    a.ValidForGroups = true;
                }
            }
        }
    }
}