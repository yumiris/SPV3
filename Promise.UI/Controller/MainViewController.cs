using System.IO;
using System.Windows;
using Promise.Library.Eula;
using Promise.Library.Halo;
using Promise.Library.Serialisation;

namespace Promise.UI.Controller
{
    class MainViewController
    {
        public void LaunchHalo()
        {
            try {
                new Eula().Inject();
                Halo haloInstace = new XmlSerialisation<Halo>().GetDeserialisedInstance("Halo_Settings.User.xml");
                new Launch(haloInstace).Start();
            } catch (FileNotFoundException) {
                MessageBox.Show("Hmmm, seems like there is no haloce.exe here.");
            } catch (IOException) {
                MessageBox.Show("Cannot inject EULA. Please run as an administrator!");
            }
        }
    }
}
