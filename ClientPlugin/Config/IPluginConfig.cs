using System.ComponentModel;

namespace ClientPlugin
{
    public interface IPluginConfig : INotifyPropertyChanged
    {
        // Disables 3rd person view
        bool DisableThirdPersonView { get; set; }
    }
}