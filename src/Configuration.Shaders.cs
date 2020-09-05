/**
 * Copyright (c) 2019 Emilian Roman
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
using HXE.SPV3;
using SPV3.Annotations;

namespace SPV3
{
  public partial class Configuration
  {
    public class ConfigurationShaders : INotifyPropertyChanged
    {
      private readonly Kernel.Configuration _configuration = new Kernel.Configuration(Paths.Kernel);

      private int  _dof                = 2;
      private bool _dynamicLensFlares  = true;
      private bool _filmGrain          = true;
      private bool _hudVisor           = true;
      private bool _lensDirt           = true;
      private int  _motionBlur         = 3;
      private int  _mxao               = 2;
      private bool _volumetricLighting = true;
      private bool _ssr                = true;

      public bool DebandAvailable
      {
        get => _configuration.Mode.Equals(HXE.Kernel.Configuration.ConfigurationMode.SPV33);
      }
      public bool DynamicFlaresAvailable
      {
        get => !_configuration.Mode.Equals(HXE.Kernel.Configuration.ConfigurationMode.SPV33);
      }
      public bool DynamicLensFlares
      {
        get => _dynamicLensFlares;
        set
        {
          if (value == _dynamicLensFlares) return;
          _dynamicLensFlares = value;
          OnPropertyChanged();
        }
      }

      public bool VolumetricLighting
      {
        get => _volumetricLighting;
        set
        {
          if (value == _volumetricLighting) return;
          _volumetricLighting = value;
          OnPropertyChanged();
        }
      }

      public bool LensDirt
      {
        get => _lensDirt;
        set
        {
          if (value == _lensDirt) return;
          _lensDirt = value;
          OnPropertyChanged();
        }
      }

      public bool HudVisor
      {
        get => _hudVisor;
        set
        {
          if (value == _hudVisor) return;
          _hudVisor = value;
          OnPropertyChanged();
        }
      }

      public bool FilmGrain
      {
        get => _filmGrain;
        set
        {
          if (value == _filmGrain) return;
          _filmGrain = value;
          OnPropertyChanged();
        }
      }

      public int MXAO
      {
        get => _mxao;
        set
        {
          if (value == _mxao) return;
          _mxao = value;
          OnPropertyChanged();
        }
      }

      public int MotionBlur
      {
        get => _motionBlur;
        set
        {
          if (value == _motionBlur) return;
          _motionBlur = value;
          OnPropertyChanged();
        }
      }

      public int DOF
      {
        get => _dof;
        set
        {
          if (value == _dof) return;
          _dof = value;
          OnPropertyChanged();
        }
      }

      public bool SSR
      {
        get => _ssr;
        set
        {
          if (value == _ssr) return;
          _ssr = value;
          OnPropertyChanged();
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      public void Save()
      {
        _configuration.Shaders.DynamicLensFlares  = DynamicLensFlares;
        _configuration.Shaders.FilmGrain          = FilmGrain;
        _configuration.Shaders.HudVisor           = HudVisor;
        _configuration.Shaders.LensDirt           = LensDirt;
        _configuration.Shaders.VolumetricLighting = VolumetricLighting;
        _configuration.Shaders.DOF                = (PostProcessing.DofOptions) DOF;
        _configuration.Shaders.MotionBlur         = (PostProcessing.MotionBlurOptions) MotionBlur;
        _configuration.Shaders.MXAO               = (PostProcessing.MxaoOptions) MXAO;
        _configuration.Shaders.SSR                = SSR;

        _configuration.Save();
      }

      public void Load()
      {
        _configuration.Load();

        DynamicLensFlares  = _configuration.Shaders.DynamicLensFlares;
        FilmGrain          = _configuration.Shaders.FilmGrain;
        HudVisor           = _configuration.Shaders.HudVisor;
        LensDirt           = _configuration.Shaders.LensDirt;
        VolumetricLighting = _configuration.Shaders.VolumetricLighting;
        DOF                = (byte) _configuration.Shaders.DOF;
        MotionBlur         = (byte) _configuration.Shaders.MotionBlur;
        MXAO               = (byte) _configuration.Shaders.MXAO;
        SSR                = _configuration.Shaders.SSR;
      }

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}