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
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using HXE;
using HXE.HCE;
using HXE.Steam;
using IWshRuntimeLibrary;
using SPV3.Annotations;
using static HXE.Paths.MCC;
using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.File;

namespace SPV3
{
  public class Install : INotifyPropertyChanged
  {
    private readonly string     _source   = Path.Combine(CurrentDirectory, "data");
    private          bool       _isDebug  = false; // Temporary: set "True" to skip Halo CE Detection and access HCE and MCC panels
    private          bool       _canInstall;
    private          Visibility _dbgPnl   = Visibility.Visible; // TODO: Implement Debug-Tools 'floating' panel
    private          Visibility _mcc      = Visibility.Collapsed;
    private          Visibility _hce      = Visibility.Collapsed;
    private          Visibility _load     = Visibility.Collapsed;
    private          Visibility _main     = Visibility.Visible;
    private          string     _status   = "Awaiting user input...";
    private          string     _target   = Path.Combine(GetFolderPath(Personal), "My Games", "Halo SPV3");
    private          string     _steamExe = Path.Combine(Steam, SteamExe);
    private          string     _steamStatus = "Find Steam.exe or its shortcut and we'll do the rest!";

    public bool IsDebug
    {
      get => _isDebug;
      set
      {
        if (value == _isDebug) return;
        _isDebug = value;
        OnPropertyChanged();
        if (value)
        {
          _dbgPnl = Visibility.Visible; 

        }
        else _dbgPnl = Visibility.Collapsed;
      }
    }

    public bool CanInstall
    {
      get => _canInstall;
      set
      {
        if (value == _canInstall) return;
        _canInstall = value;
        OnPropertyChanged();
      }
    }

    public string Status
    {
      get => _status;
      set
      {
        if (value == _status) return;
        _status = value;
        OnPropertyChanged();
      }
    }

    public string SteamExePath
    {
      get => _steamExe;
      set
      {
        if (value == _steamExe) return;
        _steamExe = value;
        OnPropertyChanged();

        if (Exists(_steamExe))
        {
          SetSteam(value);
          SetSteamStatus();
          Halo1Path = Path.Combine(SteamLibrary, SteamMccH1, Halo1dll);
          if (!Exists(Halo1Path))
          {
            try
            {
              MCC.Halo1.SetHalo1Path();
            }
            catch (Exception e)
            {
              Status = e.Message.ToLower();
            }
          }
          if (Exists(Halo1Path))
          {
            var path = Path.Combine(Registry.WoWCheck(), Registry.MSG, Registry.Custom);
            Registry.CreateKeys("Custom", path);
          }
          Status = "SPV3 successfully activated.";
        }
      }
    }

    public string SteamStatus
    {
      get => _steamStatus;
      set
      {
        if (value == _steamStatus) return;
        _steamStatus = value;
        OnPropertyChanged();
      }
    }

    public string Target
    {
      get => _target;
      set
      {
        if (value == _target) return;
        _target = value;
        OnPropertyChanged();

        /**
         * Check validity of the specified target value.
         */

        try
        {
          var exists = Directory.Exists(Target);

          if (!Directory.Exists(Target))
            Directory.CreateDirectory(Target);

          var test = Path.Combine(Target, "io.bin");
          WriteAllBytes(test, new byte[8]);
          Delete(test);

          if (!exists)
            Directory.Delete(Target);

          Status     = "Waiting for user to install SPV3.";
          CanInstall = true;
        }
        catch (Exception e)
        {
          Status     = "Installation not possible at selected path: " + e.Message.ToLower();
          CanInstall = false;
        }

        /**
         * Check available disk space. This will NOT work on UNC paths!
         */

        try
        {
          var targetDrive = Path.GetPathRoot(Target);

          foreach (var drive in DriveInfo.GetDrives())
          {
            if (!drive.IsReady || drive.Name != targetDrive) continue;

            if (drive.TotalFreeSpace > 17179869184)
            {
              CanInstall = true;
            }
            else
            {
              Status     = "Not enough disk space (16GB required) at selected path: " + Target;
              CanInstall = false;
            }
          }
        }
        catch (Exception e)
        {
          Status     = "Failed to get drive space: " + e.Message.ToLower();
          CanInstall = false;
        }

        /*
         * Prohibit installations to known problematic folders.
         */

        if (Exists(Path.Combine(Target,    HXE.Paths.HCE.Executable))
            || Exists(Path.Combine(Target, HXE.Paths.Executable))
            || Exists(Path.Combine(Target, Paths.Executable)))
        {
          Status     = "Selected folder contains existing HCE or SPV3 data. Please choose a different location.";
          CanInstall = false;
        }
      }
    }

    public Visibility Main
    {
      get => _main;
      set
      {
        if (value == _main) return;
        _main = value;
        OnPropertyChanged();
      }
    }

    public Visibility Mcc
    {
      get => _mcc;
      set
      {
        if (value == _mcc) return;
        _mcc = value;
        OnPropertyChanged();
      }
    }

    public Visibility Hce
    {
      get => _hce;
      set
      {
        if (value == _hce) return;
        _hce = value;
        OnPropertyChanged();
      }
    }

    public Visibility Load
    {
      get => _load;
      set
      {
        if (value == _load) return;
        _load = value;
        OnPropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void Initialise()
    {
      Main = Visibility.Visible;
      Mcc  = Visibility.Collapsed;
      Hce  = Visibility.Collapsed;

      /**
       * Determine if the current environment fulfills the installation requirements.
       */

      var manifest = (Manifest) Path.Combine(_source, HXE.Paths.Manifest);

      if (manifest.Exists())
      {
        Status     = "Waiting for user to install SPV3.";
        CanInstall = true;
      }
      else
      {
        Status     = "Could not find manifest in the data directory.";
        CanInstall = false;
        return;
      }

      if (!IsDebug) // USE FOR DEBUGGING HCE AND MCC PANELS. If True, skip the following If statement
      if (Detection.InferFromRegistryKeyEntry() != null) return;

      Status     = "Please install a legal copy of HCE before installing SPV3.";
      CanInstall = false;

      Main = Visibility.Collapsed;
      Mcc  = Visibility.Collapsed;
      Hce  = Visibility.Visible;
    }

    public async void Commit()
    {
      try
      {
        CanInstall = false;

        var progress = new Progress<Status>();
        progress.ProgressChanged +=
          (o, s) => Status =
            $"Installing SPV3. Please wait until this is finished! - {(decimal) s.Current / s.Total:P}";

        await Task.Run(() => { Installer.Install(_source, _target, progress); });

        /* shortcuts */
        {
          void Shortcut(string shortcutPath)
          {
            var targetFileLocation = Path.Combine(Target, Paths.Executable);

            try
            {
              var shell    = new WshShell();
              var shortcut = (IWshShortcut) shell.CreateShortcut(Path.Combine(shortcutPath, "SPV3.lnk"));

              shortcut.Description = "Single Player Version 3";
              shortcut.TargetPath  = targetFileLocation;
              shortcut.Save();
            }
            catch (Exception e)
            {
              Status = "Shortcut error: " + e.Message;
            }
          }

          var appStartMenuPath = Path.Combine
          (
            GetFolderPath(ApplicationData),
            "Microsoft",
            "Windows",
            "Start Menu",
            "Programs",
            "Single Player Version 3"
          );

          if (!Directory.Exists(appStartMenuPath))
            Directory.CreateDirectory(appStartMenuPath);

          Shortcut(GetFolderPath(DesktopDirectory));
          Shortcut(appStartMenuPath);
        }

        MessageBox.Show(
          "Installation has been successful! " +
          "Please install OpenSauce to the SPV3 folder OR Halo CE folder using AmaiSosu. Click OK to continue ...");

        try
        {
          new AmaiSosu {Path = Path.Combine(Target, Paths.AmaiSosu)}.Execute();
        }
        catch (Exception e)
        {
          Status = e.Message;
        }
        finally
        {
          Status = "Installation of SPV3 has successfully finished! " +
                   "Enjoy SPV3, and join our Discord and Reddit communities!";

          CanInstall = true;

          if (Exists(Path.Combine(Target, Paths.Executable)))
          {
            Main = Visibility.Collapsed;
            Hce  = Visibility.Collapsed;
            Load = Visibility.Visible;
          }
          else
          {
            Status = "SPV3 loader could not be found in the target directory. Please load manually.";
          }
        }
      }
      catch (Exception e)
      {
        Status     = e.Message;
        CanInstall = true;
      }
    }

    public void SetSteamStatus()
    {
      SteamStatus =
        Exists(SteamExePath) ?
        "Steam located!" :
        "Find Steam.exe or a Steam shortcut and we'll do the rest!";
    }

    public void ViewHce()
    {
      Main = Visibility.Collapsed;
      Mcc = Visibility.Collapsed;
      Hce = Visibility.Visible;
    }

    public void ViewMain()
    {
      Main = Visibility.Visible;
      Mcc = Visibility.Collapsed;
      Hce = Visibility.Collapsed;
    }

    public void ViewMcc()
    {
      Main = Visibility.Collapsed;
      Mcc = Visibility.Visible;
      Hce = Visibility.Collapsed;
    }

    public void InstallHce()
    {
      try
      {
        new Setup {Path = Path.Combine(Paths.Setup)}.Execute();
      }
      catch (Exception e)
      {
        Status = e.Message;
      }
    }

    public void InvokeSpv3()
    {
      Process.Start(new ProcessStartInfo
      {
        FileName         = Path.Combine(Target, Paths.Executable),
        WorkingDirectory = Target
      });
      Exit(0);
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}