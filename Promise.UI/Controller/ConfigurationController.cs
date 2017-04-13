using Promise.Library;
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
            var vidConfig = new VideoConfiguration(Width, Height, 60, Adapter + 1, IsWindow, IsFixedMode);
            var parConfig = new ParameterConfiguration(isSafemode: IsSafeMode);
            var hceConfig = $"{vidConfig.GetConfiguration()} {parConfig.GetConfiguration()}";

            HaloConfiguration haloVideoConfiguration = new HaloConfiguration();
            haloVideoConfiguration.WriteConfiguration(hceConfig);
        }
    }
}