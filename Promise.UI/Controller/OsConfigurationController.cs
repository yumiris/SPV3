using System.IO;
using Promise.Library.OpenSauce;
using Promise.Library.OpenSauce.Rasterizer;
using Promise.Library.OpenSauce.Rasterizer.PostProcessing;
using Promise.Library.OpenSauce.Rasterizer.ShaderExtensions;
using Promise.Library.Serialisation;
using Promise.UI.Model;
using PropertyChanged;

namespace Promise.UI.Controller
{
    [ImplementPropertyChanged]
    internal class OsConfigurationController : OsConfiguration, IConfigurationController
    {
        private readonly OpenSauceXml _osXml = new OpenSauceXml();

        public void SaveConfiguration()
        {
            var openSauce = new OpenSauce
            {
                Camera = new Camera
                {
                    FieldOfView = FieldOfView,
                    IgnoreFovChangeInCinematics = IsCinematicsFovIgnored,
                    IgnoreFovChangeInMainMenu = IsMenuFovIgnored
                },

                Rasterizer = new Rasterizer
                {
                    ShaderExtensions = new ShaderExtensions
                    {
                        IsEnabled = UseShader,
                        ShaderObject = new ShaderObject
                        {
                            IsDetailNormalMaps = UseDetailMaps,
                            IsNormalMaps = UseNormalMaps,
                            IsSpecularLighting = UseSpecularLighting,
                            IsSpecularMaps = UseSpecularMaps
                        },

                        Environment = new Environment
                        {
                            DiffuseDirectionalLightmaps = IsDiffuseDirectionalLightMaps,
                            SpecularDirectionalLightmaps = IsSpecularDirectionalLightMaps
                        },

                        Effect = new Effect {IsDepthFadeEnabled = UseDepthFade}
                    },

                    PostProcessing = new PostProcessing
                    {
                        MotionBlur = new MotionBlur {IsEnabled = UseMotionBlur},
                        Bloom = new Bloom {IsEnabled = UseBloom},
                        AntiAliasing = new AntiAliasing {IsEnabled = UseAntiAliasing},
                        ExternalEffects = new ExternalEffects {IsEnabled = UseExternalEffects},
                        MapEffects = new MapEffects {IsEnabled = UseMapEffects}
                    }
                }
            };

            var osXmlSerialisation = new XmlSerialisation<OpenSauce>();

            osXmlSerialisation.SerialiseNewXml(openSauce, _osXml.GetConfigurationFilename());
        }

        public void GetConfiguration()
        {
            var osXmlSerialisation = new XmlSerialisation<OpenSauce>();

            if (!File.Exists(_osXml.GetConfigurationFilename()))
            {
                _osXml.CreateConfigurationDirectory();
                osXmlSerialisation.SerialiseNewXml(new OpenSauce(), _osXml.GetConfigurationFilename());
            }

            var deserialisedOpenSauce = osXmlSerialisation.GetDeserialisedInstance(_osXml.GetConfigurationFilename());
            GetValuesFromInstance(deserialisedOpenSauce);
        }
    }
}