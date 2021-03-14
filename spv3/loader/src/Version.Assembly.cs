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
using SPV3.Annotations;
using static System.Reflection.Assembly;

namespace SPV3
{
  public partial class Version
  {
    public class VersionAssembly : INotifyPropertyChanged
    {
      private string _address;
      private string _content;
      private int    _version;

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

      public string Content
      {
        get => _content;
        set
        {
          if (value == _content) return;
          _content = value;
          OnPropertyChanged();
        }
      }

      public string Address
      {
        get => _address;
        set
        {
          if (value == _address) return;
          _address = value;
          OnPropertyChanged();
        }
      }

      public int Version
      {
        get => _version;
        set
        {
          if (value == _version) return;
          _version = value;
          OnPropertyChanged();
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      public void Initialise()
      {
        var versionMajor = GetEntryAssembly()?.GetName().Version.Major;

        if (versionMajor == null) return;

        var version = (int) versionMajor;
        var refHash = new Func<string>(() =>
        {
          using (var stream = GetExecutingAssembly().GetManifestResourceStream("SPV3.hash"))
          using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
            return reader.ReadToEnd();
        })();

        Version    = version;
        Content    = $"Version {version:D4}-{refHash.ToUpper()}";
        Address    = $"https://github.com/HaloSPV3/HCE/commit/{refHash}";
        Visibility = Visibility.Visible;
      }

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}