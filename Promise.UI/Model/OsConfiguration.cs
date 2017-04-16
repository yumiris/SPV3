namespace Promise.UI.Model
{
    class OsConfiguration
    {
        public bool IsShader { get; set; } = true;
        public bool IsNormalMaps { get; set; } = true;
        public bool IsSpecularMaps { get; set; } = true;
        public bool IsSpecularLighting { get; set; } = true;
        public bool IsDiffuseDirectionalLightMaps { get; set; } = true;
        public bool IsDiffuseSpecularLightMaps { get; set; } = true;
        public bool IsDepthFade { get; set; } = true;

        public bool IsMotionBlur { get; set; } = false;
        public bool IsBloom { get; set; } = true;
        public bool IsAntiAliasing { get; set; } = false;
        public bool IsExternalEffects { get; set; } = true;
        public bool IsMapEffects { get; set; } = true;

        public int FieldOfView { get; set; } = 70;
        public bool IsCinematicsFovIgnored { get; set; } = true;
        public bool IsMenuFovIgnored { get; set; } = true;
    }
}
