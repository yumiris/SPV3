using System;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Input;

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
            Stream soundEffect = Properties.Resources.Hover;
            var soundPlayer = new SoundPlayer(soundEffect);
            soundPlayer.Play();
        }

        private void ContentPresenter_MouseEnter(object sender, MouseEventArgs e)
        {
            ToggleHoverSoundEffect();
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
//            Halo haloInstance = new Halo {Configuration = new HaloConfiguration()};
//            haloInstance.Launch();
        }

        private void ConfigButton_Click(object sender, RoutedEventArgs e)
        {
            var configurationView = new ConfigurationView();
            configurationView.Show();
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
            var aboutView = new AboutView();
            aboutView.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}