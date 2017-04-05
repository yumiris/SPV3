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

        // Handlers
        private void backgroundMusic_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            backgroundMusic.Position = TimeSpan.Zero;
            backgroundMusic.Play();
        }

        // Button events
        private void LaunchButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ToggleHoverSoundEffect();
            launchButton.Source = GetBitmapImage("Resources/Graphics/Buttons/launch_hover.png");
        }

        private void LaunchButton_OnMouseLeave(object sender, MouseEventArgs e)
        {
            launchButton.Source = GetBitmapImage("Resources/Graphics/Buttons/launch.png");
        }

        private void ConfigButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ToggleHoverSoundEffect();
            configButton.Source = GetBitmapImage("Resources/Graphics/Buttons/config_hover.png");
        }

        private void ConfigButton_MouseLeave(object sender, MouseEventArgs e)
        {
            configButton.Source = GetBitmapImage("Resources/Graphics/Buttons/config.png");
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
