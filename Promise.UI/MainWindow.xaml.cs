using System;
using System.IO;
using System.Media;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Promise.UI
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

        // Button events
        private void launchButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ToggleHoverSoundEffect();
            launchButton.Source = GetBitmapImage("Resources/Graphics/Buttons/launch_hover.png");
        }

        private void launchButton_OnMouseLeave(object sender, MouseEventArgs e)
        {
            launchButton.Source = GetBitmapImage("Resources/Graphics/Buttons/launch.png");
        }

        private void configButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ToggleHoverSoundEffect();
            configButton.Source = GetBitmapImage("Resources/Graphics/Buttons/config_hover.png");
        }

        private void configButton_MouseLeave(object sender, MouseEventArgs e)
        {
            configButton.Source = GetBitmapImage("Resources/Graphics/Buttons/config.png");
        }

        private void helpButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ToggleHoverSoundEffect();
            helpButton.Source = GetBitmapImage("Resources/Graphics/Buttons/help_hover.png");
        }

        private void helpButton_MouseLeave(object sender, MouseEventArgs e)
        {
            helpButton.Source = GetBitmapImage("Resources/Graphics/Buttons/help.png");
        }

        private void aboutButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ToggleHoverSoundEffect();
            aboutButton.Source = GetBitmapImage("Resources/Graphics/Buttons/about_hover.png");
        }

        private void aboutButton_MouseLeave(object sender, MouseEventArgs e)
        {
            aboutButton.Source = GetBitmapImage("Resources/Graphics/Buttons/about.png");
        }

        private void exitButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ToggleHoverSoundEffect();
            exitButton.Source = GetBitmapImage("Resources/Graphics/Buttons/exit_hover.png");
        }

        private void exitButton_MouseLeave(object sender, MouseEventArgs e)
        {
            exitButton.Source = GetBitmapImage("Resources/Graphics/Buttons/exit.png");
        }

        // Helper functions
        private BitmapImage GetBitmapImage(string uri)
        {
            return new BitmapImage(new Uri(uri, UriKind.Relative));
        }

        private void ToggleHoverSoundEffect()
        {
            Stream soundEffect = Properties.Resources.Hover;
            SoundPlayer soundPlayer = new SoundPlayer(soundEffect);
            soundPlayer.Play();
        }
    }
}
