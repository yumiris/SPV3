using System.IO;
using Promise.Library.Halo;
using Promise.Library.Halo.Video;
using Promise.Library.Serialisation;
using Promise.UI.Model;
using PropertyChanged;

namespace Promise.UI.Controller
{
    [ImplementPropertyChanged]
    internal class HaloConfigurationController : HaloConfiguration
    {
        private readonly HaloXml _haloXml = new HaloXml();

        public VideoResolution SelectedVideoResolution { get; set; }
        public VideoRefreshRate SelectedVideoRefreshRate { get; set; }
        public int SelectedAdapterIndex { get; set; }

        public void SaveConfiguration()
        {
            Halo halo = new Halo
            {
                VideoResolution = new VideoResolution
                {
                    Width = SelectedVideoResolution.Width,
                    Height = SelectedVideoResolution.Height
                },

                VideoRefreshRate = new VideoRefreshRate {Rate = SelectedVideoRefreshRate.Rate},
                VideoAdapter = new VideoAdapter {Index = SelectedAdapterIndex + 1},

                IsWindow = IsWindow,
                IsSafeMode = IsSafeMode,
                IsFixedMode = IsFixedMode
            };

            XmlSerialisation<Halo> haloXmlSerialisation = new XmlSerialisation<Halo>();
            haloXmlSerialisation.SerialiseNewXml(halo, _haloXml.GetConfigurationFilename());
        }

        public void GetConfiguration()
        {
            XmlSerialisation<Halo> haloXmlSerialisation = new XmlSerialisation<Halo>();

            if (!File.Exists(_haloXml.GetConfigurationFilename()))
            {
                haloXmlSerialisation.SerialiseNewXml(new Halo(), _haloXml.GetConfigurationFilename());
            }

            Halo deserialisedHalo = haloXmlSerialisation.GetDeserialisedInstance(_haloXml.GetConfigurationFilename());
            GetValuesFromInstance(deserialisedHalo);
        }
    }
}