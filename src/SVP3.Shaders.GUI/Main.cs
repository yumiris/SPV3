using System.Windows;
using SPV3.Shaders;

namespace SVP3.Shaders.GUI
{
    public class Main
    {
        public Configuration Configuration { get; set; }

        public void Save()
        {
            MessageBox.Show($"{Configuration}");
        }
    }
}