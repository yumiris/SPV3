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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using HXE.SPV3;
using SPV3.Annotations;

namespace SPV3
{
  public partial class Configuration
  {
    public class ConfigurationShaders : INotifyPropertyChanged
    {
      private bool _adaptiveHDR_isReady = false;
      private bool _adaptiveHDR        = false;
      private int  _dof                = 0;
      private bool _dynamicLensFlares  = false;
      private bool _filmGrain          = false;
      private bool _hudVisor           = true;
      private bool _lensDirt           = false;
      private int  _motionBlur         = 0;
      private int  _mxao               = 0;
      private bool _volumetricLighting = false;
      private bool _ssr                = false;
      private bool _deband             = false;

      public bool ModeIsSPV33()
      {
        /**
         * TODO: Remove side-effect sometime down the line.
         */
        
        if (!System.IO.File.Exists(HXE.Paths.Legacy))
        {
          Kernel.hxe.Mode = HXE.Kernel.Configuration.ConfigurationMode.SPV33;
          return true;
        }
        else
          return false;
      }

      public bool AdaptiveHDRisReady
      {
        get => _adaptiveHDR_isReady;
        set
        {
          if (value == _adaptiveHDR_isReady) return;
          _adaptiveHDR_isReady = value;
          OnPropertyChanged();
        }
      }

      public bool AdaptiveHDR
      {
        get => _adaptiveHDR;
        set
        {
          if (value == _adaptiveHDR) return;
          _adaptiveHDR = value;
          OnPropertyChanged();
        }
      }

      // Remove when we have Halo global variables for both DLF and Deband. Still used by XML bindings.
      public Visibility DynamicFlaresAvailable
      {
        get 
        {
          return !ModeIsSPV33() ? Visibility.Visible : Visibility.Collapsed; 
        }
      }

      // Remove when we have Halo global variables for both DLF and Deband. Still used by XML bindings.
      public Visibility DebandAvailable
      {
        get
        {
          return ModeIsSPV33() ? Visibility.Visible : Visibility.Collapsed;
        }
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

      public bool Deband
      {
        get => _deband;
        set
        {
          if (value == _deband) return;
          _deband = value;
          OnPropertyChanged();
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      /// <summary>
      /// Saves this instance to SPV3.Kernel.hxe
      /// </summary>
      public void Save()
      {
        Kernel.hxe.Shaders = 0;
        
        if (DynamicLensFlares)  Kernel.hxe.Shaders |= PP.DYNAMIC_LENS_FLARES;
        if (FilmGrain)          Kernel.hxe.Shaders |= PP.FILM_GRAIN;
        if (HudVisor)           Kernel.hxe.Shaders |= PP.HUD_VISOR;
        if (LensDirt)           Kernel.hxe.Shaders |= PP.LENS_DIRT;
        if (VolumetricLighting) Kernel.hxe.Shaders |= PP.VOLUMETRIC_LIGHTING;
        if (SSR)                Kernel.hxe.Shaders |= PP.SSR;
        if (Deband)             Kernel.hxe.Shaders |= PP.DEBAND;
        if (AdaptiveHDR)        Kernel.hxe.Shaders |= PP.ADAPTIVE_HDR;
        if (DOF        == 1)    Kernel.hxe.Shaders |= PP.DOF_LOW;
        if (DOF        == 2)    Kernel.hxe.Shaders |= PP.DOF_HIGH;
        if (MotionBlur == 1)    Kernel.hxe.Shaders |= PP.MOTION_BLUR_BUILT_IN;
        if (MotionBlur == 2)    Kernel.hxe.Shaders |= PP.MOTION_BLUR_POMB_LOW;
        if (MotionBlur == 3)    Kernel.hxe.Shaders |= PP.MOTION_BLUR_POMB_HIGH;
        if (MXAO       == 1)    Kernel.hxe.Shaders |= PP.MXAO_LOW;
        if (MXAO       == 2)    Kernel.hxe.Shaders |= PP.MXAO_HIGH;
      }

      /// <summary>
      /// Loads this instance from SPV3.Kernel.hxe
      /// </summary>
      public void Load()
      {
        DynamicLensFlares  = (Kernel.hxe.Shaders & PP.DYNAMIC_LENS_FLARES) != 0;
        FilmGrain          = (Kernel.hxe.Shaders & PP.FILM_GRAIN)          != 0;
        HudVisor           = (Kernel.hxe.Shaders & PP.HUD_VISOR)           != 0;
        LensDirt           = (Kernel.hxe.Shaders & PP.LENS_DIRT)           != 0;
        VolumetricLighting = (Kernel.hxe.Shaders & PP.VOLUMETRIC_LIGHTING) != 0;
        SSR                = (Kernel.hxe.Shaders & PP.SSR)                 != 0;
        Deband             = (Kernel.hxe.Shaders & PP.DEBAND)              != 0;
        AdaptiveHDR        = (Kernel.hxe.Shaders & PP.ADAPTIVE_HDR)        != 0;

        DOF = new Func<byte>(() =>
        {
          if ((Kernel.hxe.Shaders & PP.DOF_HIGH) != 0)
            return 2;

          if ((Kernel.hxe.Shaders & PP.DOF_LOW) != 0)
            return 1;

          return 0;
        })();

        MotionBlur = new Func<byte>(() =>
        {
          if ((Kernel.hxe.Shaders & PP.MOTION_BLUR_POMB_HIGH) != 0)
            return 3;

          if ((Kernel.hxe.Shaders & PP.MOTION_BLUR_POMB_LOW) != 0)
            return 2;

          if ((Kernel.hxe.Shaders & PP.MOTION_BLUR_BUILT_IN) != 0)
            return 1;

          return 0;
        })();

        MXAO = new Func<byte>(() =>
        {
          if ((Kernel.hxe.Shaders & PP.MXAO_HIGH) != 0)
            return 2;

          if ((Kernel.hxe.Shaders & PP.MXAO_LOW) != 0)
            return 1;

          return 0;
        })();
      }

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}