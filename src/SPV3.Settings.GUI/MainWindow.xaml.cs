using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using BalsamV.Profile;
using BalsamV.Settings;
using Microsoft.Win32;

namespace SPV3.Settings.GUI
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
            FrameRateComboBox.ItemsSource = Enum.GetValues(typeof(FrameRate)).Cast<FrameRate>();
            TextureQualityComboBox.ItemsSource = Enum.GetValues(typeof(Quality)).Cast<Quality>();
            ParticlesComboBox.ItemsSource = Enum.GetValues(typeof(Particles)).Cast<Particles>();
            AudioQualityComboBox.ItemsSource = Enum.GetValues(typeof(Quality)).Cast<Quality>();
            VarietyComboBox.ItemsSource = Enum.GetValues(typeof(Quality)).Cast<Quality>();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            _main.Save();
        }

        private void Spv3Shaders(object sender, RoutedEventArgs e)
        {
            _main.Shaders();
        }
    }
}