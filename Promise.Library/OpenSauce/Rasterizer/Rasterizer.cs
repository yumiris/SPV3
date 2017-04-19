namespace Promise.Library.OpenSauce.Rasterizer
{
    public class Rasterizer
    {
        public GBuffer GBuffer { get; set; } = new GBuffer();
        public Upgrades Upgrades { get; set; } = new Upgrades();

        public ShaderExtensions.ShaderExtensions ShaderExtensions { get; set; } =
            new ShaderExtensions.ShaderExtensions();

        public PostProcessing.PostProcessing PostProcessing { get; set; } = new PostProcessing.PostProcessing();
    }
}