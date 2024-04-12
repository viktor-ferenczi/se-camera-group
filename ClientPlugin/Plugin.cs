using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using HarmonyLib;
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
            PluginSession.OverrideEnableThirdPersonView();
        }

        public void Dispose()
        {
            // TODO: Save state and close resources here, called when the game exits (not guaranteed!)
            // IMPORTANT: Do NOT call harmony.UnpatchAll() here! It may break other plugins.

            Instance = null;
        }

        public void Update()
        {
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
    }
}