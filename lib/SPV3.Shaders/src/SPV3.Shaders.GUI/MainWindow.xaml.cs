using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SPV3.Shaders.Options;

namespace SPV3.Shaders.GUI
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
            _main = (Main) DataContext;
        }

        private void AmbientOcclusionOff(object sender, RoutedEventArgs e)
        {
            _main.Configuration.AmbientOcclusion.Level = Level.Off;
            _main.Save();

            SetBackground((Button) sender, new List<Button>
            {
                AmbientOcclusionLowButton,
                AmbientOcclusionHighButton
            });
        }

        private void AmbientOcclusionLow(object sender, RoutedEventArgs e)
        {
            _main.Configuration.AmbientOcclusion.Level = Level.Low;
            _main.Save();

            SetBackground((Button) sender, new List<Button>
            {
                AmbientOcclusionOffButton,
                AmbientOcclusionHighButton
            });
        }

        private void AmbientOcclusionHigh(object sender, RoutedEventArgs e)
        {
            _main.Configuration.AmbientOcclusion.Level = Level.High;
            _main.Save();

            SetBackground((Button) sender, new List<Button>
            {
                AmbientOcclusionOffButton,
                AmbientOcclusionLowButton
            });
        }

        private void DepthOfFieldOff(object sender, RoutedEventArgs e)
        {
            _main.Configuration.DepthOfField.Level = Level.Off;
            _main.Save();

            SetBackground((Button) sender, new List<Button>
            {
                DepthOfFieldLowButton,
                DepthOfFieldHighButton
            });
        }

        private void DepthOfFieldLow(object sender, RoutedEventArgs e)
        {
            _main.Configuration.DepthOfField.Level = Level.Low;
            _main.Save();

            SetBackground((Button) sender, new List<Button>
            {
                DepthOfFieldOffButton,
                DepthOfFieldHighButton
            });
        }

        private void DepthOfFieldHigh(object sender, RoutedEventArgs e)
        {
            _main.Configuration.DepthOfField.Level = Level.High;
            _main.Save();

            SetBackground((Button) sender, new List<Button>
            {
                DepthOfFieldOffButton,
                DepthOfFieldLowButton
            });
        }

        private void DynamicFlareOff(object sender, RoutedEventArgs e)
        {
            _main.Configuration.DynamicFlare.Toggle = Toggle.Off;
            _main.Save();

            SetBackground((Button) sender, new List<Button>
            {
                DynamicFlareHighButton
            });
        }

        private void DynamicFlareHigh(object sender, RoutedEventArgs e)
        {
            _main.Configuration.DynamicFlare.Toggle = Toggle.On;
            _main.Save();

            SetBackground((Button) sender, new List<Button>
            {
                DynamicFlareOffButton
            });
        }

        private void LensDirtOff(object sender, RoutedEventArgs e)
        {
            _main.Configuration.LensDirt.Toggle = Toggle.Off;
            _main.Save();

            SetBackground((Button) sender, new List<Button>
            {
                LensDirtHighButton
            });
        }

        private void LensDirtHigh(object sender, RoutedEventArgs e)
        {
            _main.Configuration.LensDirt.Toggle = Toggle.On;
            _main.Save();

            SetBackground((Button) sender, new List<Button>
            {
                LensDirtOffButton
            });
        }

        private void EyeAdaptionOff(object sender, RoutedEventArgs e)
        {
            _main.Configuration.EyeAdaption.Toggle = Toggle.Off;
            _main.Save();

            SetBackground((Button) sender, new List<Button>
            {
                EyeAdaptionHighButton
            });
        }

        private void EyeAdaptionHigh(object sender, RoutedEventArgs e)
        {
            _main.Configuration.EyeAdaption.Toggle = Toggle.On;
            _main.Save();

            SetBackground((Button) sender, new List<Button>
            {
                EyeAdaptionOffButton
            });
        }

        private void DebandingOff(object sender, RoutedEventArgs e)
        {
            _main.Configuration.Debanding.Level = Level.Off;
            _main.Save();

            SetBackground((Button) sender, new List<Button>
            {
                DebandingLowButton,
                DebandingHighButton
            });
        }

        private void DebandingLow(object sender, RoutedEventArgs e)
        {
            _main.Configuration.Debanding.Level = Level.Low;
            _main.Save();

            SetBackground((Button) sender, new List<Button>
            {
                DebandingOffButton,
                DebandingHighButton
            });
        }

        private void DebandingHigh(object sender, RoutedEventArgs e)
        {
            _main.Configuration.Debanding.Level = Level.High;
            _main.Save();

            SetBackground((Button) sender, new List<Button>
            {
                DebandingOffButton,
                DebandingLowButton
            });
        }

        /// <summary>
        ///     Visually sets the inbound button as active, and visually deactivates the buttons in the inbound list.
        /// </summary>
        /// <param name="button">
        ///    Button to visually mark as active.
        /// </param>
        /// <param name="buttons">
        ///    Buttons to visually mark as inactive.
        /// </param>
        private static void SetBackground(Control button, List<Button> buttons)
        {
            button.Background = (Brush) new BrushConverter().ConvertFrom("#136EC6");
            buttons.ForEach(x => x.Background = Brushes.Transparent);
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        private void Close(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}