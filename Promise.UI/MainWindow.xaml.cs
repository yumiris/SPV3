using System;
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

        private void LaunchButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            launchButton.Source = GetBitmapImage("Resources/Graphics/Buttons/launch_hover.png");
        }

        private void LaunchButton_OnMouseLeave(object sender, MouseEventArgs e)
        {
            launchButton.Source = GetBitmapImage("Resources/Graphics/Buttons/launch.png");
        }

        private BitmapImage GetBitmapImage(string uri)
        {
            return new BitmapImage(new Uri(uri, UriKind.Relative));
        }

        private void backgroundMusic_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            backgroundMusic.Position = TimeSpan.Zero;
            backgroundMusic.Play();
        }



        private void TriggerSoundEffect(string soundLocation)
        {
            throw new NotImplementedException();
            //SoundPlayer soundPlayer = new SoundPlayer(soundLocation);
            //soundPlayer.Play();
        }
    }
}
