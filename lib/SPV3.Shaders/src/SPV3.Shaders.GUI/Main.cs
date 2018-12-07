using System.IO;

namespace SPV3.Shaders.GUI
{
    public class Main
    {
        public Configuration Configuration { get; set; } = new Configuration();

        public void Save()
        {
            using (var writer = new StreamWriter("initc.txt"))
                writer.WriteLine($"f0 = {ConfigurationEncoder.Encode(Configuration).Value}");
        }
    }
}