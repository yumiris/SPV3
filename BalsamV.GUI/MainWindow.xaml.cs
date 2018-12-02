using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using BalsamV.Profile;
using BalsamV.Settings;
using Microsoft.Win32;

namespace BalsamV.GUI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Main _main;

        public MainWindow()
        {
            InitializeComponent();
            InitialiseComboBoxes();

            _main = (Main) DataContext;
            _main.Initialise();
        }

        /// <summary>
        ///     Assigns the Atarashii enumerators to the combo boxes.
        /// </summary>
        private void InitialiseComboBoxes()
        {
            ColourComboBox.ItemsSource = Enum.GetValues(typeof(Colour)).Cast<Colour>();
            ConnectionComboBox.ItemsSource = Enum.GetValues(typeof(Connection)).Cast<Connection>();
            FrameRateComboBox.ItemsSource = Enum.GetValues(typeof(FrameRate)).Cast<FrameRate>();
            TextureQualityComboBox.ItemsSource = Enum.GetValues(typeof(Quality)).Cast<Quality>();
            ParticlesComboBox.ItemsSource = Enum.GetValues(typeof(Particles)).Cast<Particles>();
            AudioQualityComboBox.ItemsSource = Enum.GetValues(typeof(Quality)).Cast<Quality>();
            VarietyComboBox.ItemsSource = Enum.GetValues(typeof(Quality)).Cast<Quality>();
        }

        /// <summary>
        ///     Invokes the blam.sav file picker.
        /// </summary>
        private void Load(object sender, RoutedEventArgs routedEventArgs)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Profile Binary|blam.sav"
            };

            if (dialog.ShowDialog() == true)
                _main.Path = dialog.FileName;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            _main.Save();
        }

        private void About(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/yumiris/HCE.BalsamV");
        }

        private void Releases(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/yumiris/HCE.BalsamV/releases");
        }

        private void Version(object sender, RoutedEventArgs e)
        {
            Process.Start($"https://github.com/yumiris/HCE.BalsamV/releases/{_main.Version}");
        }
    }
}