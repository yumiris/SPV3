using System;
using System.Diagnostics;
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
            new SoundPlayer(Properties.Resources.Hover).Play();
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
            new ConfigurationView().Show();
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