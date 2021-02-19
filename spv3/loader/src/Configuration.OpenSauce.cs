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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using HXE;
using SPV3.Annotations;
using static HXE.Paths.Custom;

namespace SPV3
{
  public partial class Configuration
  {
    public class ConfigurationOpenSauce : INotifyPropertyChanged
    {
      private bool   _bloom            = true;
      private bool   _detailNormalMaps = true;
      private double _fieldOfView      = 70;
      private bool   _gBuffer          = true;
      private bool   _normalMaps       = true;
      private bool   _specularLighting = true;
      private bool   _specularMaps     = true;

      public OpenSauce Configuration { get; } = (OpenSauce) OpenSauce(Paths.Directory);

      public bool NormalMaps
      {
        get => _normalMaps;
        set
        {
          if (value == _normalMaps) return;
          _normalMaps = value;
          OnPropertyChanged();
        }
      }

      public bool DetailNormalMaps
      {
        get => _detailNormalMaps;
        set
        {
          if (value == _detailNormalMaps) return;
          _detailNormalMaps = value;
          OnPropertyChanged();
        }
      }

      public bool SpecularMaps
      {
        get => _specularMaps;
        set
        {
          if (value == _specularMaps) return;
          _specularMaps = value;
          OnPropertyChanged();
        }
      }

      public bool SpecularLighting
      {
        get => _specularLighting;
        set
        {
          if (value == _specularLighting) return;
          _specularLighting = value;
          OnPropertyChanged();
        }
      }

      public bool Bloom
      {
        get => _bloom;
        set
        {
          if (value == _bloom) return;
          _bloom = value;
          OnPropertyChanged();
        }
      }

      public double FieldOfView
      {
        get => _fieldOfView;
        set
        {
          if (value.Equals(_fieldOfView)) return;
          _fieldOfView = value;
          OnPropertyChanged();
        }
      }

      public bool GBuffer
      {
        get => _gBuffer;
        set
        {
          if (value == _gBuffer) return;
          _gBuffer = value;
          OnPropertyChanged();
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      public void Save()
      {
        Configuration.Rasterizer.GBuffer.Enabled                          = GBuffer;
        Configuration.Rasterizer.ShaderExtensions.Object.NormalMaps       = NormalMaps;
        Configuration.Rasterizer.ShaderExtensions.Object.DetailNormalMaps = DetailNormalMaps;
        Configuration.Rasterizer.ShaderExtensions.Object.SpecularMaps     = SpecularMaps;
        Configuration.Rasterizer.ShaderExtensions.Object.SpecularLighting = SpecularLighting;
        Configuration.Rasterizer.PostProcessing.Bloom.Enabled             = Bloom;
        Configuration.Camera.FieldOfView                                  = FieldOfView;
        Configuration.Save();
      }

      public void Load()
      {
        if (!Configuration.Exists())
        {
          FieldOfView = Configuration.Camera.CalculateFOV();
          return;
        }

        Configuration.Load();
        GBuffer          = Configuration.Rasterizer.GBuffer.Enabled;
        NormalMaps       = Configuration.Rasterizer.ShaderExtensions.Object.NormalMaps;
        DetailNormalMaps = Configuration.Rasterizer.ShaderExtensions.Object.DetailNormalMaps;
        SpecularMaps     = Configuration.Rasterizer.ShaderExtensions.Object.SpecularMaps;
        SpecularLighting = Configuration.Rasterizer.ShaderExtensions.Object.SpecularLighting;
        Bloom            = Configuration.Rasterizer.PostProcessing.Bloom.Enabled;
        FieldOfView      = Configuration.Camera.FieldOfView;
      }

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}