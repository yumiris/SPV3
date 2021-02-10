/**
 * Copyright (c) 2019 Emilian Roman
 * Copyright (c) 2020 Noah Sherwin
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

using System;
using System.Windows;

namespace HXE
{
  /// <summary>
  ///   Interaction logic for Settings.xaml
  /// </summary>
  public partial class Settings : Window
  {
    private          Kernel.Configuration       _configuration = new Kernel.Configuration(Paths.Configuration);
    private readonly System.Diagnostics.Process _process       = System.Diagnostics.Process.GetCurrentProcess();

    public Kernel.Configuration Configuration
    {
      get => _configuration;
      set
      {
        if (value == _configuration) return;
        _configuration = value;
      }
    }

    public Settings(Kernel.Configuration cfg)
    {
      Configuration = cfg;
      DataContext = cfg;
      Initialize();
    }

    public Settings()
    {
      Initialize();
    }

    public void Initialize()
    {
      InitializeComponent();

      Console.Info("Loading kernel settings");

      Configuration.Load();

      try { AssignConfig(); }
      catch(Exception e)
      {
        if (e.Message == "Kernel Mode not recognized.")
        {
          Configuration = new Kernel.Configuration(Configuration.Path);
          Configuration.Save();
          AssignConfig();
        }
        else throw e;
      }
    }

    public void AssignConfig()
    {
      switch (Configuration.Mode)
      {
        case Kernel.Configuration.ConfigurationMode.HCE:
          Mode.SelectedIndex = 0;
          break;
        case Kernel.Configuration.ConfigurationMode.SPV32:
          Mode.SelectedIndex = 1;
          break;
        case Kernel.Configuration.ConfigurationMode.SPV33:
          Mode.SelectedIndex = 2;
          break;
        default:
          throw new ArgumentOutOfRangeException("Switch (Configuration.Mode)", "Kernel Mode not recognized.");
      }

      MainReset.IsChecked           = Configuration.Main.Reset;
      MainPatch.IsChecked           = Configuration.Main.Patch;
      MainStart.IsChecked           = Configuration.Main.Start;
      MainResume.IsChecked          = Configuration.Main.Resume;
      MainElevated.IsChecked        = Configuration.Main.Elevated;
      TweaksCinemaBars.IsChecked    = Configuration.Tweaks.CinemaBars;
      TweaksSensor.IsChecked        = Configuration.Tweaks.Sensor;
      TweaksMagnetism.IsChecked     = Configuration.Tweaks.Magnetism;
      TweaksAutoAim.IsChecked       = Configuration.Tweaks.AutoAim;
      TweaksAcceleration.IsChecked  = Configuration.Tweaks.Acceleration;
      TweaksUnload.IsChecked        = Configuration.Tweaks.Unload;
      VideoAutoResolution.IsChecked = Configuration.Video.ResolutionEnabled == false;
      VideoUncap.IsChecked          = Configuration.Video.Uncap;
      VideoQuality.IsChecked        = Configuration.Video.Quality;
      VideoBless.IsChecked          = Configuration.Video.Bless;
      VideoGammaEnabled.IsChecked   = Configuration.Video.GammaOn;
      VideoGamma.Text               = Configuration.Video.Gamma.ToString();
      AudioQuality.IsChecked        = Configuration.Audio.Quality;
      AudioEnhancements.IsChecked   = Configuration.Audio.Enhancements;
      InputOverride.IsChecked       = Configuration.Input.Override;

      PrintConfiguration();
    }

    private void Save(object sender, RoutedEventArgs e)
    {
      Console.Info("Saving kernel settings");

      switch (Mode.SelectedIndex)
      {
        case 0:
          Configuration.Mode = Kernel.Configuration.ConfigurationMode.HCE;
          break;
        case 1:
          Configuration.Mode = Kernel.Configuration.ConfigurationMode.SPV32;
          break;
        case 2:
          Configuration.Mode = Kernel.Configuration.ConfigurationMode.SPV33;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      Configuration.Main.Reset              = MainReset.IsChecked           == true;
      Configuration.Main.Patch              = MainPatch.IsChecked           == true;
      Configuration.Main.Start              = MainStart.IsChecked           == true;
      Configuration.Main.Resume             = MainResume.IsChecked          == true;
      Configuration.Main.Elevated           = MainElevated.IsChecked        == true;
      Configuration.Tweaks.CinemaBars       = TweaksCinemaBars.IsChecked    == true;
      Configuration.Tweaks.Sensor           = TweaksSensor.IsChecked        == true;
      Configuration.Tweaks.Magnetism        = TweaksMagnetism.IsChecked     == true;
      Configuration.Tweaks.AutoAim          = TweaksAutoAim.IsChecked       == true;
      Configuration.Tweaks.Acceleration     = TweaksAcceleration.IsChecked  == true;
      Configuration.Tweaks.Unload           = TweaksUnload.IsChecked        == true;
      Configuration.Video.ResolutionEnabled = VideoAutoResolution.IsChecked == false;
      Configuration.Video.Uncap             = VideoUncap.IsChecked          == true;
      Configuration.Video.Quality           = VideoQuality.IsChecked        == true;
      Configuration.Video.Bless             = VideoBless.IsChecked          == true;
      Configuration.Video.GammaOn           = VideoGammaEnabled.IsChecked   == true;
      Configuration.Audio.Quality           = AudioQuality.IsChecked        == true;
      Configuration.Audio.Enhancements      = AudioEnhancements.IsChecked   == true;
      Configuration.Input.Override          = InputOverride.IsChecked       == true;

      try
      {
        Configuration.Video.Gamma = byte.Parse(VideoGamma.Text);
      }
      catch (Exception)
      {
        Configuration.Video.Gamma = 0;
      }

      Configuration.Save();
      Configuration.Load();

      PrintConfiguration();

      if (_process.ProcessName == "hxe")
        Exit.WithCode(Exit.Code.Success);
      else
      {
        DialogResult = true;
        Close();
      }
    }

    private void PrintConfiguration()
    {
      Console.Debug("Mode                - " + Configuration.Mode);
      Console.Debug("Main.Reset          - " + Configuration.Main.Reset);
      Console.Debug("Main.Patch          - " + Configuration.Main.Patch);
      Console.Debug("Main.Start          - " + Configuration.Main.Start);
      Console.Debug("Main.Resume         - " + Configuration.Main.Resume);
      Console.Debug("Tweaks.CinemaBars   - " + Configuration.Tweaks.CinemaBars);
      Console.Debug("Tweaks.Sensor       - " + Configuration.Tweaks.Sensor);
      Console.Debug("Tweaks.Magnetism    - " + Configuration.Tweaks.Magnetism);
      Console.Debug("Tweaks.AutoAim      - " + Configuration.Tweaks.AutoAim);
      Console.Debug("Tweaks.Acceleration - " + Configuration.Tweaks.Acceleration);
      Console.Debug("Tweaks.Unload       - " + Configuration.Tweaks.Unload);
      Console.Debug("Video.Resolution    - " + Configuration.Video.ResolutionEnabled);
      Console.Debug("Video.Uncap         - " + Configuration.Video.Uncap);
      Console.Debug("Video.Quality       - " + Configuration.Video.Quality);
      Console.Debug("Video.Bless         - " + Configuration.Video.Bless);
      Console.Debug("Video.GammaEnabled  - " + Configuration.Video.GammaOn);
      Console.Debug("Video.Gamma         - " + Configuration.Video.Gamma);
      Console.Debug("Audio.Quality       - " + Configuration.Audio.Quality);
      Console.Debug("Audio.Enhancements  - " + Configuration.Audio.Enhancements);
      Console.Debug("Input.Override      - " + Configuration.Input.Override);
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
      if (_process.ProcessName == "hxe")
        Exit.WithCode(Exit.Code.Success);
      else Hide();
    }
  }
}