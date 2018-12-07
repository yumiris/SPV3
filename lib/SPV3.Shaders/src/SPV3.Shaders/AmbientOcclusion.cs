namespace SPV3.Shaders
{
    /// <summary>
    ///     Shading/rendering technique used to calculate how each point in the scene is exposed to ambient light.
    ///     Depending on the geometry, the algorithm is used to apply dynamic shadows on occluded surfaces.
    /// </summary>
    public class AmbientOcclusion
    {
        public const int StateOff = 0x1;
        public const int StateLow = 0x2;
        public const int StateHigh = 0x4;

        /// <summary>
        ///     <see cref="Level" />
        /// </summary>
        public Level Level { get; set; }
    }
}