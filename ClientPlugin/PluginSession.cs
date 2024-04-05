using System.Collections.Generic;
using System.Linq;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage.Game;
using VRage.Game.Components;

namespace ClientPlugin
{
    // ReSharper disable once UnusedType.Global
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public class PluginSession : MySessionComponentBase
    {
        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
        }

        public override void LoadData()
        {
            base.LoadData();

            // Process the actions before they are used, enable the use of View action on groups of Camera blocks
            MyAPIGateway.TerminalControls.CustomActionGetter += CustomActionGetter;
        }

        private static void CustomActionGetter(IMyTerminalBlock block, List<IMyTerminalAction> actions)
        {
            if (!(block is IMyCameraBlock))
                return;

            var action = actions.FirstOrDefault(a => a.Name.ToString() == "View");
            if (action == null)
            {
                return;
            }

            action.ValidForGroups = true;
        }
    }
}