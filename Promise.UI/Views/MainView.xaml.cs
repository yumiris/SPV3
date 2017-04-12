using System;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Windows;
using Promise.Library;
using Application = System.Windows.Application;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

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
            SoundPlayer soundPlayer = new SoundPlayer(soundEffect);
            soundPlayer.Play();
        }

        private void ContentPresenter_MouseEnter(object sender, MouseEventArgs e)
        {
            ToggleHoverSoundEffect();
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            Configuration configuration = new Configuration();
            Halo halo = new Halo();
            halo.LaunchGame(configuration.ReadConfiguration());
        }

        private void ConfigButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigView configView = new ConfigView();
            configView.Show();
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
            AboutView aboutView = new AboutView();
            aboutView.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}