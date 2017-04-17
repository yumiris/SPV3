namespace Promise.UI.Model
{
    class OsConfiguration
    {
        public int FieldOfView { get; set; } = 70;
        public bool IsCinematicsFovIgnored { get; set; } = true;
        public bool IsMenuFovIgnored { get; set; } = true;

        public bool UseShader { get; set; } = true;
        public bool UseNormalMaps { get; set; } = true;
        public bool UseSpecularMaps { get; set; } = true;
        public bool UseSpecularLighting { get; set; } = true;
        public bool IsDiffuseDirectionalLightMaps { get; set; } = true;
        public bool IsDiffuseSpecularLightMaps { get; set; } = true;
        public bool UseDepthFade { get; set; } = true;

        public bool UseMotionBlur { get; set; } = false;
        public bool UseBloom { get; set; } = true;
        public bool UseAntiAliasing { get; set; } = false;
        public bool UseExternalEffects { get; set; } = true;
        public bool UseMapEffects { get; set; } = true;
    }
}
