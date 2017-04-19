namespace Promise.Library.OpenSauce.Rasterizer.PostProcessing
{
    public class PostProcessing
    {
        public MotionBlur MotionBlur { get; set; } = new MotionBlur();
        public Bloom Bloom { get; set; } = new Bloom();
        public AntiAliasing AntiAliasing { get; set; } = new AntiAliasing();
        public ExternalEffects ExternalEffects { get; set; } = new ExternalEffects();
        public MapEffects MapEffects { get; set; } = new MapEffects();
    }
}