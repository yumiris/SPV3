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
using System.Collections.Generic;
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
using static System.Windows.Visibility;
using System.Linq;
using static System.Reflection.Assembly;

namespace SPV3
{
  public class Install : INotifyPropertyChanged
  {
    private readonly string     _source   = Path.Combine(CurrentDirectory, "data");
    private          bool       _canInstall;
    private          bool       _compress = false;
    //private readonly Visibility _dbgPnl   = Debug.IsDebug ? Visible : Collapsed; // TODO: Implement Debug-Tools 'floating' panel, Move this to Main.cs
    private          Visibility _activation = Collapsed;
    private          Visibility _load     = Collapsed;
    private          Visibility _main     = Visible;
    private          string     _status   = "Awaiting user input...";
    private          string     _target   = Path.Combine(GetFolderPath(Personal), "My Games", "Halo SPV3");
    private          string     _steamExe = Path.Combine(Steam, SteamExe);
    private          string     _steamStatus = "Find Steam.exe or its shortcut and we'll do the rest!";

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

    public bool Compress
    {
      get => _compress;
      set
      {
        if (value == _compress) return;
        _compress = value;
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

        CheckSteamPath(value);
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
        if (string.IsNullOrEmpty(Target) || !Directory.Exists(Target))
        {
          Status = "Enter a valid path.";
          CanInstall = false;
          return;
        }

        if (Target.Contains(GetFolderPath(ProgramFiles)) 
        || !string.IsNullOrEmpty(GetFolderPath(ProgramFilesX86)) && Target.Contains(GetFolderPath(ProgramFilesX86)) )
        {
          Status = "The game does not function correctly when install to Program Files. Please choose a difference location.";
          CanInstall = false;
          return;
        }


        try
        {
          var exists     = Directory.Exists(Target);
          var path       = Target;
          var rootExists = Directory.Exists(Path.GetPathRoot(Target));

          if (!exists && !rootExists)
          {
            throw new DirectoryNotFoundException(Target);
          }
          if (!exists && rootExists)
          {
            while (!Directory.Exists(path))
            {
              path = Directory.GetParent(path).Name;
              if (path == "Debug") return;
            }
          }

          // if Target and Root exist...
          _target = Path.GetFullPath(_target);
          value = Path.GetFullPath(value);
          var test = Path.Combine(path, "io.bin");
          WriteAllBytes(test, new byte[8]);
          Delete(test);

          Status     = "Waiting for user to install SPV3.";
          CanInstall = true;
        }
        catch (Exception e)
        {
          var msg = "Installation not possible at selected path: " + Target + "\n Error: " + e.ToString() + "\n";
          var log = (HXE.File)Paths.Exception;
          log.AppendAllText(msg);
          Status = msg;
          CanInstall = false;
          return;
        }

        /**
         * Check available disk space. This will NOT work on UNC paths!
         */
        try
        {

          /** First, check the C:\ drive to ensure there's enough free space 
            * for temporary extraction to %temp% */
          if (Directory.Exists(@"C:\"))
          {
            var systemDrive = new DriveInfo(@"C:\");
            if (systemDrive.TotalFreeSpace < 10737418240)
            { 
              Status     = @"Not enough disk space (10GB required) on the C:\ drive. " + 
                            "Clear junk files using Disk Cleanup or allocate more space to the volume";
              CanInstall = false;
              return;
            }
          }

          /** 
            * Check if the target drive has at least 16GB of free space 
            */
          var targetDrive = new DriveInfo(Path.GetPathRoot(Target));

          if (targetDrive.IsReady && targetDrive.TotalFreeSpace > 17179869184)
          {
            CanInstall = true;
          }
          else
          {
            Status     = "Not enough disk space (16GB required) at selected path: " + Target;
            CanInstall = false;
            return;
          }
        }
        catch (Exception e)
        {
          var msg = "Failed to get drive space.\n Error:  " + e.ToString() + "\n";
          var log = (HXE.File)Paths.Exception;
          log.AppendAllText(msg);
          Status     = msg;
          CanInstall = false;
        }

        /**
         * Prohibit installations to known problematic folders.
         */
        if (Exists(Path.Combine(Target, HXE.Paths.HCE.Executable))
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

    public Visibility Activation
    {
      get => _activation;
      set
      {
        if (value == _activation) return;
        _activation = value;
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
      Main = Visible;
      Activation = Collapsed;

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

      if (Registry.GameExists("Custom")
       || Registry.GameExists("Retail")
       || ( Kernel.hxe.Tweaks.Patches & Patcher.EXEP.DISABLE_DRM_AND_KEY_CHECKS) == 1)
        return;
      // else, prompt for activation

      Status     = "Please install a legal copy of Halo 1 before installing SPV3.";
      CanInstall = false;

      Main        = Collapsed;
      Activation  = Visible;

      if (Exists(SteamExePath))
        CheckSteamPath(SteamExePath);
    }

    public async void Commit()
    {
      try
      {
        CanInstall = false;

        var progress = new Progress<Status>();
        progress.ProgressChanged +=
          (o, s) => Status =
            "Installing SPV3. Please wait until this is finished!";

        await Task.Run(() => { SFX.Extract(new SFX.Configuration
        {
          Target     = new DirectoryInfo(Target),
          Executable = new FileInfo(GetExecutingAssembly().Location)
        }); });

        /* MCC DRM Patch */
        if ((Kernel.hxe.Tweaks.Patches & Patcher.EXEP.DISABLE_DRM_AND_KEY_CHECKS) != 0)
          new Patcher().Write(Kernel.hxe.Tweaks.Patches, Path.Combine(Target, HXE.Paths.HCE.Executable));

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
              shortcut.WorkingDirectory = Target;
              shortcut.Save();
            }
            catch (Exception e)
            {
              var msg = "Shortcut error.\n Error:  " + e.ToString() + "\n";
              var log = (HXE.File)Paths.Exception;
              log.AppendAllText(msg);
              Status = msg;
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
          var msg = "Failed to install OpenSauce via Amai Sosu.\n Error:  " + e.ToString() + "\n";
          var log = (HXE.File)Paths.Exception;
          log.AppendAllText(msg);
          Status = msg;
        }
        finally
        {
          Status = "Installation of SPV3 has successfully finished! " +
                   "Enjoy SPV3, and join our Discord and Reddit communities!";

          CanInstall = true;

          if (Exists(Path.Combine(Target, Paths.Executable)))
          {
            Main = Collapsed;
            Activation  = Collapsed;
            Load = Visible;
          }
          else
          {
            Status = "SPV3 loader could not be found in the target directory. Please load manually.";
          }
        }
      }
      catch (Exception e)
      {
        var msg = "Failed to install SPV3.\n Error:  " + e.ToString() + "\n";
        var log = (HXE.File)Paths.Exception;
        log.AppendAllText(msg);
        Status     = msg;
        CanInstall = true;
      }
    }

    public void Update_SteamStatus()
    {
      SteamStatus =
        Exists(_steamExe) ?
        "Steam located!" :
        "Find Steam.exe or a Steam shortcut and we'll do the rest!";
    }

    public void CheckSteamPath(string exe)
    {
      if (Exists(exe) && exe.Contains("steam.exe"))
      {
        SetSteam(exe);
        Update_SteamStatus();
        Halo1Path = Path.Combine(SteamLibrary, SteamMccH1, Halo1dll);

        if (!Exists(Halo1Path))
        {
          try
          {
            MCC.Halo1.SetHalo1Path();
          }
          catch (Exception e)
          {
            SteamStatus = "Failed to find CEA";
            var msg = SteamStatus + NewLine
                    + " Error: " + e.Message + NewLine;
            var log = (HXE.File) Paths.Exception;
            log.AppendAllText(msg);
            return;
          }
        }

        if (Exists(Halo1Path))
        {
          Kernel.hxe.Tweaks.Patches |= Patcher.EXEP.DISABLE_DRM_AND_KEY_CHECKS;
          Status = "Halo CEA Located via Steam." + NewLine
                 + "Waiting for user to install SPV3." + NewLine;
          CanInstall = true;
          Main       = Visible;
          Activation = Collapsed;
        }
        else
          SteamStatus = "Steam Located, but Halo CEA not found.";
      }
      else
        Update_SteamStatus();
    }

    public void ViewActivation() // Debug widget
    {
      Main = Collapsed;
      Activation  = Visible;
    }

    public void ViewMain()
    {
      Main = Visible;
      Activation  = Collapsed;
    }

    public void InstallHce()
    {
      try
      {
        new Setup {Path = Path.Combine(Paths.Setup)}.Execute();
      }
      catch (Exception e)
      {
        var msg  = "Failed to install Halo Custom Edition." + NewLine
                 + " Error:  " + e.ToString() + NewLine;
        var log  = (HXE.File) Paths.Exception;
        var ilog = (HXE.File) Paths.Install;
        log.AppendAllText(msg);
        ilog.AppendAllText(msg);
        Status = "Failed to install Halo Custom Edition." + NewLine
               + " Error:  " + e.Message;
      }
    }

    public void IsHaloOrCEARunning()
    {
      var processes = new List<Process>();
      processes.AddRange(Process.GetProcessesByName("halo"));
      processes.AddRange(Process.GetProcessesByName("haloce"));
      processes.AddRange(Process.GetProcessesByName("MCC-Win64-Shipping"));
      var hpc = processes.Any(Process => Process.MainModule.FileVersionInfo.FileVersion == "01.00.10.0621");
      var mcc = false;

      if (Process.GetProcessesByName("MCC-Win64-Shipping").Count() != 0)
      {
        System.Collections.ReadOnlyCollectionBase mods = Process.GetProcessesByName("MCC-Win64-Shipping").First().Modules;
        foreach (ProcessModule mod in mods)
        {
          mcc = mod.ModuleName == Halo1dll;
          if (mcc)
            break;
        }
      }

      if (Debug.IsDebug)
      {
        var file = (HXE.File) Paths.Install;
        var text = string.Empty;
        if (processes.Count != 0)
          foreach (var proc in processes)
          {
            text += proc.MainWindowTitle + NewLine;
          }
        else text = "None found.";
        file.AppendAllText(text + NewLine + "=== END PROCS ===" + NewLine + NewLine);
      }

      if (processes.Count != 0)
      {
        if (hpc)
        {
          Kernel.hxe.Tweaks.Patches |= Patcher.EXEP.DISABLE_DRM_AND_KEY_CHECKS;
          CanInstall = true;
          Main       = Visible;
          Activation = Collapsed;
          Status     = "Process Detection: Halo PC Found" + NewLine
                     + "Waiting for user to install SPV3.";
          return;
        }
        else if (mcc)
        {
          Kernel.hxe.Tweaks.Patches |= Patcher.EXEP.DISABLE_DRM_AND_KEY_CHECKS;
          CanInstall = true;
          Main       = Visible;
          Activation = Collapsed;
          Status     = "Process Detection: MCC CEA Found" + NewLine
                     + "Waiting for user to install SPV3.";
          return;
        }
        else Status = "Process Detection: MCC Found, but CEA not present";
        return;
      }
      else Status = "Process Detection: No Matching Processes" + NewLine 
                  + "No MCC (with CEA), Halo Retail, or Custom Edition processes found.";
      return;
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