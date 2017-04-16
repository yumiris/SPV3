using Promise.Library.OpenSauce;
using Promise.Library.OpenSauce.Rasterizer;
using Promise.Library.OpenSauce.Rasterizer.PostProcessing;
using Promise.Library.OpenSauce.Rasterizer.ShaderExtensions;
using Promise.UI.Model;
using PropertyChanged;

namespace Promise.UI.Controller
{
    [ImplementPropertyChanged]
    internal class OsConfigurationController : OsConfiguration
    {
        public void SaveData()
        {
            OpenSauce openSauce = new OpenSauce
            {
                Camera = new Camera
                {
                    FieldOfView = FieldOfView,
                    IgnoreFovChangeInCinematics = IsCinematicsFovIgnored,
                    IgnoreFovChangeInMainMenu = IsMenuFovIgnored
                },
                Rasterizer = new Rasterizer
                {
                    GBuffer = new GBuffer { IsEnabled = true },
                    Upgrades = new Upgrades { IsMaximumRenderedTrianglesEnabled = true },
                    ShaderExtensions = new ShaderExtensions
                    {
                        IsEnabled = IsShader,
                        Environment = new Environment
                        {
                            DiffuseDirectionalLightmaps = IsDiffuseDirectionalLightMaps,
                            SpecularDirectionalLightmaps = IsDiffuseSpecularLightMaps
                        },
                        Effect = new Effect { IsDepthFadeEnabled = IsDepthFade }
                    },
                    PostProcessing = new PostProcessing
                    {
                        MotionBlur = new MotionBlur
                        {
                            IsEnabled = IsMotionBlur,
                            BlurAmount = 0.005
                        },
                        Bloom = new Bloom { IsEnabled = IsBloom },
                        AntiAliasing = new AntiAliasing { IsEnabled = IsAntiAliasing },
                        ExternalEffects = new ExternalEffects() { IsEnabled = IsExternalEffects },
                        MapEffects = new MapEffects { IsEnabled = IsMapEffects }
                    }
                }
            };

            OpenSauceConfiguration openSauceConfiguration = new OpenSauceConfiguration {OpenSauce = openSauce};
            openSauceConfiguration.SaveData();
        }
    }
}
