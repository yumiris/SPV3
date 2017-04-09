using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

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

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}
