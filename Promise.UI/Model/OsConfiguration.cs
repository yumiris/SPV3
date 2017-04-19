using Promise.Library.OpenSauce;

namespace Promise.UI.Model
{
    internal class OsConfiguration
    {
        public int FieldOfView { get; set; } = 70;
        public bool IsCinematicsFovIgnored { get; set; } = true;
        public bool IsMenuFovIgnored { get; set; } = true;

        public bool UseShader { get; set; } = true;
        public bool UseNormalMaps { get; set; } = true;
        public bool UseDetailMaps { get; set; } = true;
        public bool UseSpecularMaps { get; set; } = true;
        public bool UseSpecularLighting { get; set; } = true;
        public bool IsDiffuseDirectionalLightMaps { get; set; } = true;
        public bool IsSpecularDirectionalLightMaps { get; set; } = true;
        public bool UseDepthFade { get; set; } = true;

        public bool UseMotionBlur { get; set; }
        public bool UseBloom { get; set; } = true;
        public bool UseAntiAliasing { get; set; }
        public bool UseExternalEffects { get; set; } = true;
        public bool UseMapEffects { get; set; } = true;

        protected void GetValuesFromInstance(OpenSauce openSauce)
        {
            FieldOfView = openSauce.Camera.FieldOfView;
            IsCinematicsFovIgnored = openSauce.Camera.IgnoreFovChangeInCinematics;
            IsMenuFovIgnored = openSauce.Camera.IgnoreFovChangeInMainMenu;

            UseShader = openSauce.Rasterizer.ShaderExtensions.IsEnabled;
            UseNormalMaps = openSauce.Rasterizer.ShaderExtensions.ShaderObject.IsNormalMaps;
            UseDetailMaps = openSauce.Rasterizer.ShaderExtensions.ShaderObject.IsDetailNormalMaps;
            UseSpecularMaps = openSauce.Rasterizer.ShaderExtensions.ShaderObject.IsSpecularMaps;
            UseSpecularLighting = openSauce.Rasterizer.ShaderExtensions.ShaderObject.IsSpecularLighting;
            IsDiffuseDirectionalLightMaps = openSauce.Rasterizer.ShaderExtensions.Environment.DiffuseDirectionalLightmaps;
            IsSpecularDirectionalLightMaps = openSauce.Rasterizer.ShaderExtensions.Environment.SpecularDirectionalLightmaps;
            UseDepthFade = openSauce.Rasterizer.ShaderExtensions.Effect.IsDepthFadeEnabled;

            UseMotionBlur = openSauce.Rasterizer.PostProcessing.MotionBlur.IsEnabled;
            UseBloom = openSauce.Rasterizer.PostProcessing.Bloom.IsEnabled;
            UseAntiAliasing = openSauce.Rasterizer.PostProcessing.AntiAliasing.IsEnabled;
            UseExternalEffects = openSauce.Rasterizer.PostProcessing.ExternalEffects.IsEnabled;
            UseMapEffects = openSauce.Rasterizer.PostProcessing.MapEffects.IsEnabled;
        }
    }
}