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

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using SPV3.Annotations;
using static System.Windows.Forms.Screen;

namespace SPV3
{
  public partial class Configuration
  {
    public class ConfigurationLoader : INotifyPropertyChanged
    {
      private const int Length = 256;

      private byte   _adapter;                                          /* physical monitor to run hce/spv3 on        */
      private bool   _borderless;                                       /* run hce/spv3 without window borders        */
      private bool   _borderlessEnabled;                                /* ability to toggle borderless               */
      private bool   _cinemabars = false;                               /* toggle spv3 cinematic black bars           */
      private bool   _doom;                                             /* toggle spv3 doom mode                      */
      private bool   _eax;                                              /* toggle hw accel. & environmental sound     */
      private byte   _framerate = 60;                                   /* framerate to run spv3 at (in vsync mode)   */
      private byte   _gamma     = 150;                                  /* gamma level to run spv3 at (in vsync mode) */
      private ushort _height    = (ushort) PrimaryScreen.Bounds.Height; /* height spv3/hce will be displayed at       */
      private byte   _mode;                                             /* display - fullscreen/window/borderless     */
      private bool   _native;                                           /* runs native instead of custom resolution   */
      private bool   _photo;                                            /* enables spv3 photo/blind mode              */
      private byte   _preference = 1;                                   /* video preference (stable vs hertz unlocked */
      private bool   _preset     = true;                                /* use the built-in spv3 controller preset    */
      private bool   _resolutionEnabled;                                /* ability to provide custom resolution       */
      private bool   _shaders = true;                                   /* toggle spv3 post-processing effects        */
      private ushort _width   = (ushort) PrimaryScreen.Bounds.Width;    /* width spv3/hce will be displayed at        */
      private bool   _window;                                           /* runs spv3/hce as a windowed application    */
      private bool   _elevated;                                         /* runs spv3/hce in elevated (admin) mode     */

      public bool Native
      {
        get => _native;
        set
        {
          if (value == _native) return;
          _native = value;
          OnPropertyChanged();
          UpdateResolutionEnabled();
        }
      }

      public byte Mode
      {
        get => _mode;
        set
        {
          if (value == _mode) return;
          _mode = value;
          OnPropertyChanged();
          UpdateWindowBorderless();
        }
      }

      public bool Window
      {
        get => _window;
        set
        {
          if (value == _window) return;
          _window = value;
          OnPropertyChanged();
          UpdateBorderlessEnabled();
        }
      }

      public ushort Width
      {
        get => _width;
        set
        {
          if (value == _width) return;
          _width = value;
          OnPropertyChanged();
        }
      }

      public ushort Height
      {
        get => _height;
        set
        {
          if (value == _height) return;
          _height = value;
          OnPropertyChanged();
        }
      }

      public bool DOOM
      {
        get => _doom;
        set
        {
          if (value == _doom) return;
          _doom = value;
          OnPropertyChanged();
        }
      }

      public bool Photo
      {
        get => _photo;
        set
        {
          if (value == _photo) return;
          _photo = value;
          OnPropertyChanged();
        }
      }

      public bool Preset
      {
        get => _preset;
        set
        {
          if (value == _preset) return;
          _preset = value;
          OnPropertyChanged();
        }
      }

      public bool Shaders
      {
        get => _shaders;
        set
        {
          if (value == _shaders) return;
          _shaders = value;
          OnPropertyChanged();
        }
      }

      public byte Framerate
      {
        get => _framerate;
        set
        {
          if (value == _framerate) return;
          _framerate = value;
          OnPropertyChanged();
        }
      }

      public byte Preference
      {
        get => _preference;
        set
        {
          if (value == _preference) return;
          _preference = value;
          OnPropertyChanged();
          UpdateBorderlessEnabled();
        }
      }

      public bool EAX
      {
        get => _eax;
        set
        {
          if (value == _eax) return;
          _eax = value;
          OnPropertyChanged();
        }
      }

      public byte Gamma
      {
        get => _gamma;
        set
        {
          if (value == _gamma) return;
          _gamma = value;
          OnPropertyChanged();
        }
      }

      public byte Adapter
      {
        get => _adapter;
        set
        {
          if (value == _adapter) return;
          _adapter = value;
          OnPropertyChanged();
        }
      }

      public bool Cinematic
      {
        get => _cinematic;
        set
        {
          if (value == _cinematic) return;
          _cinematic = value;
          OnPropertyChanged();
        }
      }

      public bool Borderless
      {
        get => _borderless;
        set
        {
          if (value == _borderless) return;
          _borderless = value;
          OnPropertyChanged();
        }
      }

      public bool BorderlessEnabled
      {
        get => _borderlessEnabled;
        set
        {
          if (value == _borderlessEnabled) return;
          _borderlessEnabled = value;
          OnPropertyChanged();
        }
      }

      public bool ResolutionEnabled
      {
        get => _resolutionEnabled;
        set
        {
          if (value == _resolutionEnabled) return;
          _resolutionEnabled = value;
          OnPropertyChanged();
        }
      }

      public bool Elevated
      {
        get => _elevated;
        set
        {
          if (value == _elevated) return;
          _elevated = value;
          OnPropertyChanged();
          UpdateBorderlessEnabled();
        }
      }

      public List<string> Adapters => AllScreens
        .Select
        (
          screen => screen.DeviceName
            .Substring(4)
            .Replace("DISPLAY", "Display ")
        ).ToList();

      public event PropertyChangedEventHandler PropertyChanged;

      public void UpdateBorderlessEnabled()
      {
        BorderlessEnabled = Preference == 1 && Elevated == false;
      }

      public void UpdateWindowBorderless()
      {
        Borderless = _mode == 2;
        Window     = _mode == 1 || _mode == 2;
      }

      public void UpdateResolutionEnabled()
      {
        ResolutionEnabled = Native == false;
      }

      public void Save()
      {
        using (var fs = new FileStream(Paths.Configuration, FileMode.Create, FileAccess.Write))
        using (var ms = new MemoryStream(Length))
        using (var bw = new BinaryWriter(ms))
        {
          /* padding */
          {
            bw.Write(new byte[Length]);
            ms.Position = 0;
          }

          /* signature */
          {
            bw.Write(Encoding.Unicode.GetBytes("~yumiris"));
          }

          /* padding */
          {
            bw.Write(new byte[16 - ms.Position]);
          }

          /* video */
          {
            bw.Write(Shaders);
            bw.Write(Window);
            bw.Write(Width);
            bw.Write(Height);
            bw.Write(Framerate);
            bw.Write(Preference);
            bw.Write(Gamma);
            bw.Write(Adapter);
          }

          /* modes */
          {
            bw.Write(DOOM);
            bw.Write(Photo);
            bw.Write(Borderless);
          }

          /* tweaks */
          {
            bw.Write(EAX);
            bw.Write(Preset);
            bw.Write(Cinematic);
            bw.Write(Elevated);
          }

          /* display mode */
          {
            bw.Write(Mode);
            bw.Write(Native);
          }

          ms.Position = 0;
          ms.CopyTo(fs);
        }
      }

      public void Load()
      {
        if (!Exists())
          return;

        using (var fs = new FileStream(Paths.Configuration, FileMode.Open, FileAccess.Read))
        using (var ms = new MemoryStream(Length))
        using (var br = new BinaryReader(ms))
        {
          fs.CopyTo(ms);
          ms.Position = 0;

          /* padding */
          {
            ms.Position += 16 - ms.Position;
          }

          /* video */
          {
            Shaders    = br.ReadBoolean();
            Window     = br.ReadBoolean();
            Width      = br.ReadUInt16();
            Height     = br.ReadUInt16();
            Framerate  = br.ReadByte();
            Preference = br.ReadByte();
            Gamma      = br.ReadByte();
            Adapter    = br.ReadByte();
          }

          /* modes */
          {
            DOOM       = br.ReadBoolean();
            Photo      = br.ReadBoolean();
            Borderless = br.ReadBoolean();
          }

          /* tweaks */
          {
            EAX       = br.ReadBoolean();
            Preset    = br.ReadBoolean();
            Cinematic = br.ReadBoolean();
            Elevated  = br.ReadBoolean();
          }

          /* display mode */
          {
            Mode   = br.ReadByte();
            Native = br.ReadBoolean();
          }
        }

        UpdateBorderlessEnabled();
        UpdateResolutionEnabled();
      }

      public bool Exists()
      {
        return File.Exists(Paths.Configuration);
      }

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}
