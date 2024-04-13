using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using HarmonyLib;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.FileSystem;
using VRage.Plugins;

namespace ClientPlugin
{
    // ReSharper disable once UnusedType.Global
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class Plugin : IPlugin
    {
        public const string Name = "CameraGroup";
        private const string ConfigFileName = "CameraGroup.cfg";

        public static Plugin Instance { get; private set; }
        public static PersistentConfig<PluginConfig> Config;

        private static string cameraInfoGridName;
        private static string cameraInfoBlockName;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public void Init(object gameInstance)
        {
            Instance = this;

            var configPath = Path.Combine(MyFileSystem.UserDataPath, ConfigFileName);
            Config = PersistentConfig<PluginConfig>.Load(configPath);
            Config.Data.PropertyChanged += OnPropertyChanged;

            var harmony = new Harmony(Name);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private static void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OverrideEnableThirdPersonView();
        }

        public void Dispose()
        {
            // TODO: Save state and close resources here, called when the game exits (not guaranteed!)
            // IMPORTANT: Do NOT call harmony.UnpatchAll() here! It may break other plugins.

            Instance = null;
        }

        public void Update()
        {
            if (cameraInfoGridName != null)
            {
                MyHud.CameraInfo.Enable(cameraInfoGridName, cameraInfoBlockName);

                cameraInfoGridName = null;
                cameraInfoBlockName = null;
            }

            if (MySession.Static != null && MySession.Static.GameplayFrameCounter % 60 == 0)
            {
                OverrideEnableThirdPersonView();
            }
        }

        private static void OverrideEnableThirdPersonView()
        {
            if (MySession.Static?.Settings == null)
                return;

            if (MySession.Static.CameraController != null &&
                MyGuiScreenGamePlay.Static != null &&
                Config.Data.DisableThirdPersonView &&
                !MySession.Static.CameraController.IsInFirstPersonView)
            {
                MyGuiScreenGamePlay.Static.SwitchCamera();
            }

            MySession.Static.Settings.Enable3rdPersonView = PluginSession.OriginalEnable3rdPersonView && !Config.Data.DisableThirdPersonView;
        }

        // ReSharper disable once UnusedMember.Global
        public void OpenConfigDialog()
        {
            MyGuiSandbox.AddScreen(new PluginConfigDialog());
        }

        //TODO: Uncomment and use this method to load asset files
        /*public void LoadAssets(string folder)
        {

        }*/

        public static void ShowCameraInfoLater(string gridName, string blockName)
        {
            cameraInfoGridName = gridName;
            cameraInfoBlockName = blockName;
        }
    }
}