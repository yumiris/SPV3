/**
 * Copyright (c) 2018-2019 Emilian Roman
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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using HCE.Eaxir.Annotations;
using SPV3.CLI;
using File = System.IO.File;

namespace HCE.Eaxir
{
  public class Main : INotifyPropertyChanged
  {
    private string _hceGamePath;

    private Profile _profile;
    private string  _profileName;
    private string  _profilePath;

    public string HceGamePath
    {
      get => _hceGamePath;
      set
      {
        if (value == _hceGamePath) return;
        _hceGamePath = value;
        OnPropertyChanged();
      }
    }

    public string ProfileName
    {
      get => _profileName;
      set
      {
        if (value == _profileName) return;
        _profileName = value;
        OnPropertyChanged();
      }
    }

    public string ProfilePath
    {
      get => _profilePath;
      set
      {
        if (value == _profilePath) return;
        _profilePath = value;
        OnPropertyChanged();
      }
    }

    public Profile Profile
    {
      get => _profile;
      set
      {
        if (Equals(value, _profile)) return;
        _profile = value;
        OnPropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void LoadHceGamePath()
    {
      HceGamePath = Path.GetDirectoryName(Executable.Detect());
    }

    public void LoadProfile()
    {
      Profile = Profile.Detect();

      ProfileName = Profile.Details.Name;
      ProfilePath = Profile.Path;
    }

    public void Save()
    {
      CopyExisting();
      InstallDSOAL();
      PatchProfile();

      void CopyExisting()
      {
        var files = new List<string>
        {
          Path.Combine(HceGamePath, "alsoft.ini"),
          Path.Combine(HceGamePath, "dsoal-aldrv.dll"),
          Path.Combine(HceGamePath, "dsound.dll")
        };

        foreach (var file in files)
          if (File.Exists(file))
            File.Move(file, file + ".bak-" + Guid.NewGuid());
      }

      void InstallDSOAL()
      {
        ZipFile.ExtractToDirectory("DSOAL.zip", HceGamePath);
      }

      void PatchProfile()
      {
        _profile.Audio.HWA = true;
        _profile.Audio.EAX = true;
        _profile.Save();
      }
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}