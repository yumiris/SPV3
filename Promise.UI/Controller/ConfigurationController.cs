using Promise.Library.Configuration;
using Promise.UI.Model;
using PropertyChanged;

namespace Promise.UI.Controller
{
    [ImplementPropertyChanged]
    internal class ConfigurationController : Configuration
    {
        public void SaveConfiguration()
        {
            // Fixes dropdown's zero-based index.
            var chosenAdapter = Adapter + 1;

            var videoConfig = new VideoConfiguration(Width, Height, 60, chosenAdapter, IsWindow, IsFixedMode);
            var paramConfig = new ParameterConfiguration(isSafemode: IsSafeMode);

            var configurationData = $"{videoConfig.GetConfiguration()} {paramConfig.GetConfiguration()}";

            var haloVideoConfiguration = new HaloConfiguration();
            haloVideoConfiguration.WriteConfiguration(configurationData);
        }
    }
}