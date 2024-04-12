using System.Diagnostics.CodeAnalysis;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using VRage.Game.Components;

namespace ClientPlugin
{
    // ReSharper disable once UnusedType.Global
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class PluginSession : MySessionComponentBase
    {
        private static bool originalEnable3RdPersonView;

        public override void LoadData()
        {
            base.LoadData();

            originalEnable3RdPersonView = MySession.Static.Settings.Enable3rdPersonView;

            OverrideEnableThirdPersonView();
        }

        public static void OverrideEnableThirdPersonView()
        {
            if (MySession.Static?.Settings == null)
                return;

            if (MySession.Static.CameraController != null &&
                MyGuiScreenGamePlay.Static != null &&
                Plugin.Config.Data.DisableThirdPersonView &&
                !MySession.Static.CameraController.IsInFirstPersonView)
            {
                MyGuiScreenGamePlay.Static.SwitchCamera();
            }

            MySession.Static.Settings.Enable3rdPersonView = originalEnable3RdPersonView && !Plugin.Config.Data.DisableThirdPersonView;
        }
    }
}