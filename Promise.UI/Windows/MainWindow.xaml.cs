using System;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Input;

namespace Promise.UI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
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
            throw new NotImplementedException();
        }

        private void ConfigButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
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
            Windows.AboutWindow aboutWindow = new Windows.AboutWindow();
            aboutWindow.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}
