using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Sandbox.Game.Gui;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Weapons;

namespace ClientPlugin.Patches
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
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
                    // Enable this action for groups of blocks
                    a.ValidForGroups = true;
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(MethodType.Constructor)]
        public static void ConstructorPostfix(ref MyToolbar ___m_toolbar)
        {
            // Turrets add an empty toolbar, which blocks out the cockpit's one.
            // Since this toolbar is always empty and cannot be configured by the player
            // it can just be removed, so it does not interfere.
            ___m_toolbar = null;
        }
    }
}