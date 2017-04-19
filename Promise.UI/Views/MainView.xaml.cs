using System;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Input;
using Promise.Library.Eula;
using Promise.Library.Halo;
using Promise.Library.Serialisation;
using Promise.UI.Views.Configuration;

namespace Promise.UI.Views
{
    /// <summary>
    ///     Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void ToggleHoverSoundEffect()
        {
            new SoundPlayer(Properties.Resources.Hover).Play();
        }

        private void ContentPresenter_MouseEnter(object sender, MouseEventArgs e)
        {
            ToggleHoverSoundEffect();
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new Eula().Inject();

                var haloInstace = new XmlSerialisation<Halo>().GetDeserialisedInstance("Halo_Settings.User.xml");
                new Launch(haloInstace).Start();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Hmmm, seems like there is no haloce.exe here.");
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "No EULA found, which Halo requires. Attempting to inject it resulted in failure. Please run as an administrator!");
            }
        }

        private void ConfigButton_Click(object sender, RoutedEventArgs e)
        {
            new HaloConfigurationView().Show();
        }

        private void CommunityButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.reddit.com/r/halospv3/");
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            new AboutView().Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}