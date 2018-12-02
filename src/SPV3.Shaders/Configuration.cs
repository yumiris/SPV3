using SPV3.Shaders.PPEs;

namespace SPV3.Shaders
{
    /// <summary>
    ///     Type that represents the user's shader preferences.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        ///     <see cref="AmbientOcclusion" />
        /// </summary>
        public AmbientOcclusion AmbientOcclusion { get; set; } = new AmbientOcclusion();

        /// <summary>
        ///     <see cref="DepthOfField" />
        /// </summary>
        public DepthOfField DepthOfField { get; set; } = new DepthOfField();

        /// <summary>
        ///     <see cref="DynamicFlare" />
        /// </summary>
        public DynamicFlare DynamicFlare { get; set; } = new DynamicFlare();

        /// <summary>
        ///     <see cref="LensDirt" />
        /// </summary>
        public LensDirt LensDirt { get; set; } = new LensDirt();

        /// <summary>
        ///     <see cref="EyeAdaption" />
        /// </summary>
        public EyeAdaption EyeAdaption { get; set; } = new EyeAdaption();

        /// <summary>
        ///     <see cref="AntiAliasing" />
        /// </summary>
        public AntiAliasing AntiAliasing { get; set; } = new AntiAliasing();

        /// <summary>
        ///     <see cref="Debanding" />
        /// </summary>
        public Debanding Debanding { get; set; } = new Debanding();
    }
}