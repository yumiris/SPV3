using System;
using System.Diagnostics;
using System.Media;
using System.Windows;
using System.Windows.Input;
using Promise.UI.Controller;
using Promise.UI.Views.Configuration;

namespace Promise.UI.Views
{
    /// <summary>
    ///     Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        private readonly MainViewController _mainViewController = new MainViewController();

        public MainView()
        {
            InitializeComponent();
            DataContext = _mainViewController;
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
            _mainViewController.LaunchHalo();
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
            throw new NotImplementedException("Help screen hasn't been developed yet.");
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