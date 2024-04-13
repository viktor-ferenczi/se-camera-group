using System.Diagnostics.CodeAnalysis;
using Sandbox.Game.World;
using VRage.Game.Components;

namespace ClientPlugin
{
    // ReSharper disable once UnusedType.Global
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class PluginSession : MySessionComponentBase
    {
        public static bool OriginalEnable3rdPersonView = true;

        public override void BeforeStart()
        {
            base.BeforeStart();

            OriginalEnable3rdPersonView = MySession.Static.Settings.Enable3rdPersonView;
        }
    }
}