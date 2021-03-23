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
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SPV3
{
  public partial class Configuration_UserControl : UserControl
  {
    private readonly Configuration _configuration;

    public Configuration_UserControl()
    {
      InitializeComponent();
      _configuration = (Configuration) DataContext;
      _configuration.Load();
    }

    public event EventHandler Home;

    private void Save(object sender, RoutedEventArgs e)
    {
      _configuration.Save();
      Home?.Invoke(sender, e);
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
      _configuration.Load();
      Home?.Invoke(sender, e);
    }

    private void CalculateFOV(object sender, RoutedEventArgs e)
    {
      _configuration.CalculateFOV();
    }

    private void ResetWeaponPositions(object sender, RoutedEventArgs e)
    {
      _configuration.ResetWeaponPositions();
    }

    private void InstallOpenSauce(object sender, RoutedEventArgs e)
    {
      try
      {
        new AmaiSosu{}.Execute();
      }
      catch (Exception exception)
      {
        MessageBox.Show(exception.Message);
      }
    }

    private void ShowInputPresetWindow(object sender, RoutedEventArgs e)
    {
      ControllerPreset controllerPresetWindow = new ControllerPreset ();
      controllerPresetWindow.Show();
    }

    private void GBufferChanged(object sender, RoutedEventArgs e)
    {
      if ((bool)!GBuffer_CheckBox.IsChecked)
        MessageBox.Show("WARNING: VISR and Thermal Vision will not work. " +
                        "This should only be used on low-end computers as a last resort.");
    }

    private void AdaptiveHDRChanged(object sender, RoutedEventArgs e)
    {
      if ((bool)!AdaptiveHDR_CheckBox.IsChecked)
      {
        _configuration.OpenSauce.Bloom = false;
        _configuration.Shaders.SSR = false;
        MessageBox.Show("WARNING: Bloom and Screen Space Reflections require Adaptive HDR to render correctly.");
        Update_AdaptiveHDRisReady(sender, e);
      }
    }

    private void Update_AdaptiveHDRisReady(object sender, RoutedEventArgs e)
    {
      if (_configuration == null) return;
      _configuration.Shaders.AdaptiveHDRisReady = true == _configuration.Loader.Shaders == _configuration.Shaders.AdaptiveHDR;
    }

    private void PresetVeryLow(object sender, RoutedEventArgs e)
    {
      _configuration.OpenSauce.GBuffer            = false;
      _configuration.Shaders.AdaptiveHDR          = true;
      _configuration.Shaders.HudVisor             = false;
      _configuration.Shaders.FilmGrain            = false;
      _configuration.Shaders.VolumetricLighting   = false;
      _configuration.Shaders.LensDirt             = false;
      _configuration.Shaders.DynamicLensFlares    = false;
      _configuration.Shaders.MotionBlur           = 0;
      _configuration.Shaders.DOF                  = 0;
      _configuration.Shaders.MXAO                 = 0;
      _configuration.Shaders.SSR                  = false;
      _configuration.Shaders.Deband               = false;
      _configuration.OpenSauce.NormalMaps         = false;
      _configuration.OpenSauce.DetailNormalMaps   = false;
      _configuration.OpenSauce.SpecularMaps       = false;
      _configuration.OpenSauce.SpecularLighting   = false;
      _configuration.OpenSauce.Bloom              = false;
      _configuration.Chimera.Interpolation        = 0;
      _configuration.Chimera.AnisotropicFiltering = false;
      _configuration.Chimera.BlockLOD             = false;
    }

    private void PresetLow(object sender, RoutedEventArgs e)
    {
      _configuration.OpenSauce.GBuffer            = true;
      _configuration.Shaders.AdaptiveHDR          = true;
      _configuration.Shaders.HudVisor             = false;
      _configuration.Shaders.FilmGrain            = false;
      _configuration.Shaders.VolumetricLighting   = true;
      _configuration.Shaders.LensDirt             = true;
      _configuration.Shaders.DynamicLensFlares    = false;
      _configuration.Shaders.MotionBlur           = 0;
      _configuration.Shaders.DOF                  = 0;
      _configuration.Shaders.MXAO                 = 0;
      _configuration.Shaders.SSR                  = false;
      _configuration.Shaders.Deband               = false;
      _configuration.OpenSauce.NormalMaps         = true;
      _configuration.OpenSauce.DetailNormalMaps   = false;
      _configuration.OpenSauce.SpecularMaps       = false;
      _configuration.OpenSauce.SpecularLighting   = true;
      _configuration.OpenSauce.Bloom              = false;
      _configuration.Chimera.Interpolation        = 8;
      _configuration.Chimera.AnisotropicFiltering = false;
      _configuration.Chimera.BlockLOD             = false;
    }

    private void PresetMedium(object sender, RoutedEventArgs e)
    {
      _configuration.OpenSauce.GBuffer            = true;
      _configuration.Shaders.AdaptiveHDR          = true;
      _configuration.Shaders.HudVisor             = true;
      _configuration.Shaders.FilmGrain            = false;
      _configuration.Shaders.VolumetricLighting   = true;
      _configuration.Shaders.LensDirt             = true;
      _configuration.Shaders.DynamicLensFlares    = false;
      _configuration.Shaders.MotionBlur           = 1;
      _configuration.Shaders.DOF                  = 1;
      _configuration.Shaders.MXAO                 = 0;
      _configuration.Shaders.SSR                  = false;
      _configuration.Shaders.Deband               = true;
      _configuration.OpenSauce.NormalMaps         = true;
      _configuration.OpenSauce.DetailNormalMaps   = true;
      _configuration.OpenSauce.SpecularMaps       = true;
      _configuration.OpenSauce.SpecularLighting   = true;
      _configuration.OpenSauce.Bloom              = true;
      _configuration.Chimera.Interpolation        = 8;
      _configuration.Chimera.AnisotropicFiltering = true;
      _configuration.Chimera.BlockLOD             = false;
    }

    private void PresetHigh(object sender, RoutedEventArgs e)
    {
      _configuration.OpenSauce.GBuffer            = true;
      _configuration.Shaders.AdaptiveHDR          = true;
      _configuration.Shaders.HudVisor             = true;
      _configuration.Shaders.FilmGrain            = true;
      _configuration.Shaders.VolumetricLighting   = true;
      _configuration.Shaders.LensDirt             = true;
      _configuration.Shaders.DynamicLensFlares    = false;
      _configuration.Shaders.MotionBlur           = 2;
      _configuration.Shaders.DOF                  = 1;
      _configuration.Shaders.MXAO                 = 1;
      _configuration.Shaders.SSR                  = false;
      _configuration.Shaders.Deband               = true;
      _configuration.OpenSauce.NormalMaps         = true;
      _configuration.OpenSauce.DetailNormalMaps   = true;
      _configuration.OpenSauce.SpecularMaps       = true;
      _configuration.OpenSauce.SpecularLighting   = true;
      _configuration.OpenSauce.Bloom              = true;
      _configuration.Chimera.Interpolation        = 8;
      _configuration.Chimera.AnisotropicFiltering = true;
      _configuration.Chimera.BlockLOD             = false;
    }

    private void PresetVeryHigh(object sender, RoutedEventArgs e)
    {
      _configuration.OpenSauce.GBuffer            = true;
      _configuration.Shaders.AdaptiveHDR          = true;
      _configuration.Shaders.HudVisor             = true;
      _configuration.Shaders.FilmGrain            = true;
      _configuration.Shaders.VolumetricLighting   = true;
      _configuration.Shaders.LensDirt             = true;
      _configuration.Shaders.DynamicLensFlares    = false;
      _configuration.Shaders.MotionBlur           = 3;
      _configuration.Shaders.DOF                  = 2;
      _configuration.Shaders.MXAO                 = 2;
      _configuration.Shaders.Deband               = true;
      _configuration.OpenSauce.NormalMaps         = true;
      _configuration.OpenSauce.DetailNormalMaps   = true;
      _configuration.OpenSauce.SpecularMaps       = true;
      _configuration.OpenSauce.SpecularLighting   = true;
      _configuration.Shaders.SSR                  = false;
      _configuration.OpenSauce.Bloom              = true;
      _configuration.Chimera.Interpolation        = 8;
      _configuration.Chimera.AnisotropicFiltering = true;
      _configuration.Chimera.BlockLOD             = false;
    }

    private void PresetUltra(object sender, RoutedEventArgs e)
    {
      _configuration.OpenSauce.GBuffer            = true;
      _configuration.Shaders.AdaptiveHDR          = true;
      _configuration.Shaders.HudVisor             = true;
      _configuration.Shaders.FilmGrain            = true;
      _configuration.Shaders.VolumetricLighting   = true;
      _configuration.Shaders.LensDirt             = true;
      _configuration.Shaders.DynamicLensFlares    = true;
      _configuration.Shaders.MotionBlur           = 3;
      _configuration.Shaders.DOF                  = 2;
      _configuration.Shaders.MXAO                 = 2;
      _configuration.Shaders.SSR                  = true;
      _configuration.Shaders.Deband               = true;
      _configuration.OpenSauce.NormalMaps         = true;
      _configuration.OpenSauce.DetailNormalMaps   = true;
      _configuration.OpenSauce.SpecularMaps       = true;
      _configuration.OpenSauce.SpecularLighting   = true;
      _configuration.OpenSauce.Bloom              = true;
      _configuration.Chimera.Interpolation        = 8;
      _configuration.Chimera.AnisotropicFiltering = true;
      _configuration.Chimera.BlockLOD             = false;
    }

    private void ShowAdvanced(object sender, RoutedEventArgs e)
    {
      _configuration.ShowHxeSettings();
    }

    private void ShowPositions(object sender, RoutedEventArgs e)
    {
      _configuration.ShowHxeWepPositions();
    }

    private void EaxChanged(object sender, RoutedEventArgs e)
    {
      if (_configuration.Loader.EAX)
        MessageBox.Show("WARNING: EAX sound enhancements coincide with certain " +
                        "crashes. Please do not report sound effect crashes when EAX is ON.");
    }

    private void SSR_Or_ResolutionChanged(object sender, RoutedEventArgs e)
    {
      if (_configuration == null)
        return;

      if (!_configuration.Shaders.SSR)
        return;

      if (!int.TryParse(ResolutionWidth.Text, out var width))
        return;

      if (!int.TryParse(ResolutionHeight.Text, out var height))
        return;

      if (width > 2160 || height > 1080)
        MessageBox.Show
        (
          "WARNING: With SSR on and your preferred resolution, you may not get stable FPS. " +
          "We recommend keeping resolution below 2160x1080 with SSR on."
        );
    }
  }
}