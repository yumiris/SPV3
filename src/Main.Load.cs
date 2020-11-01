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

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using HXE;
using HXE.HCE;
using HXE.SPV3;
using SPV3.Annotations;
using File = System.IO.File;

namespace SPV3
{
  public partial class Main
  {
    public class MainLoad : INotifyPropertyChanged
    {
      private Visibility _visibility = Visibility.Collapsed;

      public Visibility Visibility
      {
        get => _visibility;
        set
        {
          if (value == _visibility) return;
          _visibility = value;
          OnPropertyChanged();
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      public void Invoke()
      {
        var spv3      = new Configuration.ConfigurationLoader();                 /* for configuration          */
        var hxe       = new Kernel.Configuration(Paths.Kernel);                  /* for compatibility & tweaks */
        var openSauce = (OpenSauce) HXE.Paths.Custom.OpenSauce(Paths.Directory); /* for menu fixes, gfx, modes */
        var chimera   = (Chimera) HXE.Paths.Custom.Chimera(Paths.Directory);     /* for interpolation          */

        if (spv3.Exists())
        {
          spv3.Load();
        }
        else
        {
          spv3.Preset  = true;
          spv3.Shaders = true;
        }

        hxe.Load();
        hxe.Mode               = Kernel.Configuration.ConfigurationMode.SPV33;
        hxe.Tweaks.Sensor      = true;          /* forcefully enable motion sensor   */
        hxe.Main.Reset         = true;          /* improve loading stability         */
        hxe.Main.Patch         = true;          /* improve loading stability         */
        hxe.Main.Resume        = true;          /* improve loading stability         */
        hxe.Main.Start         = true;          /* improve loading stability         */
        hxe.Main.Elevated      = spv3.Elevated; /* prevent certain crashes           */
        hxe.Video.Resolution   = true;          /* permit custom resolution override */
        hxe.Video.Quality      = false;         /* permit in-game quality settings   */
        hxe.Video.Uncap        = spv3.Preference == 1;
        hxe.Video.Gamma        = spv3.Gamma;
        hxe.Video.Bless        = spv3.Borderless && spv3.Window && spv3.Preference == 1 && spv3.Elevated == false;
        hxe.Audio.Enhancements = spv3.EAX;
        hxe.Input.Override     = spv3.Preset;
        hxe.Tweaks.CinemaBars  = spv3.CinemaBars;
        hxe.Tweaks.Unload      = !spv3.Shaders;

        if (File.Exists(HXE.Paths.Version))
        {
          hxe.Mode = Kernel.Configuration.ConfigurationMode.SPV33;
        }
        if (chimera.Exists())
        {
          chimera.Load();
        }
        else
        {
          chimera.Interpolation        = 8;
          chimera.AnisotropicFiltering = false;
          chimera.UncapCinematic       = true;
          chimera.BlockLOD             = false;
        }

        if (openSauce.Exists())
          openSauce.Load();
        else
          openSauce.Camera.CalculateFOV();

        openSauce.HUD.ShowHUD                                       = true;
        openSauce.Rasterizer.PostProcessing.MapEffects.Enabled      = false; /* for opensauce to interpret as true  */
        openSauce.Rasterizer.PostProcessing.ExternalEffects.Enabled = true;  /* for opensauce to interpret as false */

        if (spv3.DOOM && !spv3.Photo)
          if (File.Exists(Paths.DOOM))
          {
            openSauce.Objects.Weapon.Load(Paths.DOOM);
            openSauce.HUD.ShowHUD = true;
          }

        if (spv3.Photo && !spv3.DOOM)
          if (File.Exists(Paths.Photo))
          {
            openSauce.Objects.Weapon.Load(Paths.Photo);
            openSauce.HUD.ShowHUD = false;
            hxe.Tweaks.Sensor     = false;
          }

        if (openSauce.Camera.FieldOfView < 40 || openSauce.Camera.FieldOfView > 180)
          openSauce.Camera.CalculateFOV();

        spv3.Save();      /* saves to %APPDATA%\SPV3 */
        openSauce.Save(); /* saves to %APPDATA%\SPV3 */
        chimera.Save();   /* saves to %APPDATA%\SPV3 */
        hxe.Save();       /* saves to %APPDATA%\SPV3 */

        Kernel.Invoke(new Executable
        {
          Path = Path.Combine(Environment.CurrentDirectory, HXE.Paths.HCE.Executable),
          Profile = new Executable.ProfileOptions
          {
            Path = Paths.Directory
          },
          Video = new Executable.VideoOptions
          {
            Mode    = spv3.Preference == 1 && spv3.Framerate > 0,
            Width   = spv3.Native ? (ushort) 0 : spv3.Width,
            Height  = spv3.Native ? (ushort) 0 : spv3.Height,
            Refresh = spv3.Framerate,
            Window  = spv3.Window,
            Adapter = (byte) (spv3.Adapter + 1),
            NoGamma = hxe.Video.Gamma == 0
          },
          Debug = new Executable.DebugOptions
          {
            Console    = true,
            Developer  = true,
            Screenshot = true,
            Initiation = Paths.Initiation
          },
          Miscellaneous = new Executable.MiscellaneousOptions
          {
            NoVideo = true
          }
        }, hxe);
      }

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}
