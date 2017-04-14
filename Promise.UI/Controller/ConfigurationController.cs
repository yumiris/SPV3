using Promise.Library.Configuration;
using Promise.Library.Utilities;
using Promise.Library.Video;
using Promise.UI.Model;
using PropertyChanged;
using Configuration = Promise.UI.Model.Configuration;

namespace Promise.UI.Controller
{
    [ImplementPropertyChanged]
    internal class ConfigurationController : Configuration
    {
        public VideoResolution SelectedVideoResolution { get; set; }
        public VideoRefreshRate SelectedVideoRefreshRate { get; set; }
        public int SelectedAdapterIndex { get; set; }

        public void SaveConfiguration()
        {
            var chosenWidth = SelectedVideoResolution.Width;
            var chosenHeight = SelectedVideoResolution.Height;
            var chosenRate = SelectedVideoRefreshRate.Rate;
            var chosenAdapter = SelectedAdapterIndex + 1;

            var videoConfig = new DisplayConfiguration(chosenWidth, chosenHeight, chosenRate, chosenAdapter, IsWindow,
                IsFixedMode);
            var paramConfig = new ParameterConfiguration(isSafemode: IsSafeMode);

            var configurationData = $"{videoConfig.GetConfiguration()} {paramConfig.GetConfiguration()}";

            var haloVideoConfiguration = new ConfigOperation();
            haloVideoConfiguration.WriteConfiguration(configurationData);
        }
    }
}