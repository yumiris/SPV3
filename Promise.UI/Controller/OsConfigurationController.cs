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
                    ShaderExtensions = new ShaderExtensions
                    {
                        IsEnabled = UseShader,
                        Environment = new Environment
                        {
                            DiffuseDirectionalLightmaps = IsDiffuseDirectionalLightMaps,
                            SpecularDirectionalLightmaps = IsDiffuseSpecularLightMaps
                        },
                        Effect = new Effect { IsDepthFadeEnabled = UseDepthFade }
                    },
                    PostProcessing = new PostProcessing
                    {
                        MotionBlur = new MotionBlur { IsEnabled = UseMotionBlur },
                        Bloom = new Bloom { IsEnabled = UseBloom },
                        AntiAliasing = new AntiAliasing { IsEnabled = UseAntiAliasing },
                        ExternalEffects = new ExternalEffects { IsEnabled = UseExternalEffects },
                        MapEffects = new MapEffects { IsEnabled = UseMapEffects }
                    }
                }
            };

            OpenSauceConfiguration openSauceConfiguration = new OpenSauceConfiguration {OpenSauce = openSauce};
            openSauceConfiguration.SaveData();
        }
    }
}
