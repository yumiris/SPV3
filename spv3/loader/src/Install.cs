/**
 * Copyright (c) 2019 Emilian Roman
 * Copyright (c) 2021 Noah Sherwin
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
using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.File;
using static System.Windows.Visibility;
using static HXE.Paths.MCC;
using Process = System.Diagnostics.Process;

namespace SPV3
{
    public class Install : INotifyPropertyChanged
    {
        private const string _ssdRec = "SPV3 must be installed to an SSD. Otherwise, you will experience loading hitches.";
        private readonly string _source = Path.Combine(CurrentDirectory, "data");
        private bool _canInstall;
        private bool _compress = false;

        //private readonly Visibility _dbgPnl   = Debug.IsDebug ? Visible : Collapsed; // TODO: Implement Debug-Tools 'floating' panel, Move this to Main.cs
        private Visibility _activation = Collapsed;

        private Visibility _load = Collapsed;
        private Visibility _main = Visible;
        private string _status = _ssdRec;
        private string _target = Path.Combine(GetFolderPath(Personal), "My Games", "Halo SPV3");
        private string _steamExe = Path.Combine(HXE.Paths.Steam.Directory, HXE.Paths.Steam.SteamExe);
        private string _steamStatus = "Find Steam.exe or its shortcut and we'll do the rest!";
        private string _winStoreStatus = "Choose the drive where Halo MCC CEA is located!";

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

        public string WinStoreStatus
        {
            get => _winStoreStatus;
            set
            {
                if (value == _winStoreStatus) return;
                _winStoreStatus = value;
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
                ValidateTarget(value);
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

            ValidateTarget(Target);

            /** Determine if the current environment fulfills the installation requirements. */
            var manifest = (Manifest) Path.Combine(_source, HXE.Paths.Manifest);

            if (!manifest.Exists())
            {
                Status = "Could not find manifest in the data directory.";
                CanInstall = false;
                return;
            }

            /** Check Game Activation */

            bool CustomActivated = Registry.GameActivated(Registry.Game.Custom);
            bool RetailActivated = Registry.GameActivated(Registry.Game.Retail);

            /** If DRM Patch enabled, stop */
            if ((Kernel.hxe.Tweaks.Patches & Patcher.EXEP.DISABLE_DRM_AND_KEY_CHECKS) == 1)
            {
                outputInfo();
                return;
            }

            /** If Custom Edition is already activated, stop. */
            if (CustomActivated)
            {
                outputInfo();
                return;
            }

            /** Activate SPV3 if Retail is installed, then stop */
            if (RetailActivated)
            {
                Activate("Halo Retail located.");
                outputInfo();
                return;
            }

            /** Passively detect Steam MCC CEA */
            if (Exists(SteamExePath))
            {
                CheckSteamPath(SteamExePath);
                outputInfo();
                return;
            }

            /** Passively detect Modifiable UWP MCC CEA */
            if (string.IsNullOrEmpty(Halo1Path) || !Exists(Halo1Path))
            {
                CheckMCCWinStorePath();
                outputInfo();
                return;
            }

            /** else, prompt for activation */
            outputInfo();
            Status = "Please install a legal copy of Halo 1 before installing SPV3.";
            CanInstall = false;
            Main = Collapsed;
            Activation = Visible;

            void outputInfo()
            {
                HXE.File file = (HXE.File) Paths.Install;
                string output = string.Empty;
                output += $"INFO -- DRM patch queued: {(Kernel.hxe.Tweaks.Patches & Patcher.EXEP.DISABLE_DRM_AND_KEY_CHECKS) == 1}{NewLine}";
                output += $"INFO -- Custom Edition is activated: {CustomActivated}{NewLine}";
                output += $"INFO -- Retail Edition is activated: {RetailActivated}{NewLine}";
                output += $"INFO -- CEA found: {Exists(Halo1Path)}{NewLine}";
                file.WriteAllText(output);
            }
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

                try
                {
                    await Task.Run(() => { Installer.Install(_source, _target, progress, Compress); });
                }
                catch (InvalidOperationException)
                {
                    if (!Debug.IsDebug)
                        throw;
                }

                /** MCC DRM Patch */

                try
                {
                    if ((Kernel.hxe.Tweaks.Patches & Patcher.EXEP.DISABLE_DRM_AND_KEY_CHECKS) != 0)
                        new Patcher().Write(Kernel.hxe.Tweaks.Patches, Path.Combine(Target, HXE.Paths.HCE.Executable));
                }
                catch (FileNotFoundException)
                {
                    if (!Debug.IsDebug)
                        throw;
                }

                /** shortcuts */
                {
                    void Shortcut(string shortcutPath)
                    {
                        var targetFileLocation = Path.Combine(Target, Paths.Executable);

                        try
                        {
                            var shell = new WshShell();
                            var shortcut = (IWshShortcut) shell.CreateShortcut(Path.Combine(shortcutPath, "SPV3.lnk"));

                            shortcut.Description = "Single Player Version 3";
                            shortcut.TargetPath = targetFileLocation;
                            shortcut.WorkingDirectory = Target;
                            shortcut.Save();
                        }
                        catch (Exception e)
                        {
                            var msg = "Shortcut error.\n Error:  " + e.ToString() + "\n";
                            var log = (HXE.File) Paths.Exception;
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

                /** Install OpenSauce via AmaiSosu */
                try
                {
                    var amaiSosu = new AmaiSosu { Path = Path.Combine(Target, Paths.AmaiSosu) };
                    while (!Exists(Path.Combine(GetFolderPath(CommonApplicationData), "Kornner Studios", "Halo CE", "OpenSauceUI.pak"))
                        && !Exists(Path.Combine(GetFolderPath(CommonApplicationData), "Kornner Studios", "Halo CE", "shaders", "gbuffer_shaders.shd"))
                        && !Exists(Path.Combine(GetFolderPath(CommonApplicationData), "Kornner Studios", "Halo CE", "shaders", "pp_shaders.shd")))
                    {
                        try
                        {
                            amaiSosu.Execute();
                            if (!amaiSosu.Exists())
                                MessageBox.Show("Click OK after AmaiSosu is installed.");
                        }
                        catch (Exception e)
                        {
                            if (e.Message == "The operation was canceled by the user") // RunAs elevation not granted
                                continue;
                            else throw;
                        }
                    }
                }
                catch (Exception e)
                {
                    var msg = "Failed to install OpenSauce via Amai Sosu.\n Error:  " + e.ToString() + "\n";
                    var log = (HXE.File) Paths.Exception;
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
                        Activation = Collapsed;
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
                var log = (HXE.File) Paths.Exception;
                log.AppendAllText(msg);
                Status = msg;
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
            if (Exists(exe) && exe.Equals("steam.exe", StringComparison.OrdinalIgnoreCase))
            {
                HXE.Paths.Steam.SetSteam(exe);
                Update_SteamStatus();
                Halo1Path = Path.Combine(HXE.Paths.Steam.Library, HTMCC, H1Dir, H1dll);

                if (Exists(Halo1Path))
                    Activate("Steam MCC CEA found in default-ish location.");
                else
                    try
                    {
                        SteamStatus = "Searching for and validating Halo CEA's files...";
                        MCC.Halo1.SetHalo1Path(MCC.Halo1.Platform.Steam);
                        if (Exists(Halo1Path))
                            Activate("Halo CEA Located via Steam.");
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

                if (!Exists(Halo1Path))
                    SteamStatus = "Steam Located, but Halo CEA not found.";
            }
            else
                Update_SteamStatus();
        }

        public void CheckMCCWinStorePath(string drive = null)
        {
            /** SET or GET UWP MCC's Halo1.dll path */
            if (drive == null)
                MCC.Halo1.SetHalo1Path(MCC.Halo1.Platform.WinStore);
            else
            {
                Halo1Path = Path.Combine(drive, UwpH1DllPath);
            }

            /** If UWP MCC's Halo1.dll exists, queue up the HCE DRM patch */
            if (Path.IsPathRooted(Halo1Path) && Exists(Halo1Path))
                Activate("Halo CEA Located via WinStore Mod.");
            else
            {
                WinStoreStatus = "Failed to find CEA on the drive";
                var msg = WinStoreStatus + NewLine
                        + " Error: " + "Could not find CEA for Winstore on " + drive + NewLine;
                var log = (HXE.File) Paths.Exception;
                log.AppendAllText(msg);
            }
        }

        public void ValidateTarget(string path)
        {
            /** Check validity of the specified target value.
             *
             */
            if (string.IsNullOrEmpty(path)
              || !Directory.Exists(Path.GetPathRoot(path)))
            {
                Status = "Enter a valid path.";
                CanInstall = false;
                return;
            }

            try
            {
                var exists = Directory.Exists(path);
                var root = Path.GetPathRoot(path);
                var rootExists = Directory.Exists(root);

                if (!exists && rootExists)
                {
                    while (!Directory.Exists(path))
                    {
                        path = Directory.GetParent(path).FullName;
                        if (path == CurrentDirectory)
                        {
                            Status = "Enter a valid path.";
                            CanInstall = false;
                            return;
                        }
                    }
                }

                // if Target and Root exist...
                path = Path.GetFullPath(path);
                var test = Path.Combine(path, "io.bin");
                WriteAllBytes(test, new byte[8]);
                Delete(test);

                Status = _ssdRec;
                CanInstall = true;
            }
            catch (Exception e)
            {
                var msg = "Installation not possible at selected path: " + path + "\n Error: " + e.ToString() + "\n";
                var log = (HXE.File) Paths.Exception;
                log.AppendAllText(msg);
                Status = msg;
                CanInstall = false;
                return;
            }

            /** Check if the target is in Program Files or Program Files (x86)
             *
             */
            if (path.Contains(GetFolderPath(ProgramFiles))
            || !string.IsNullOrEmpty(GetFolderPath(ProgramFilesX86))
            && path.Contains(GetFolderPath(ProgramFilesX86)))
            {
                Status = "The game does not function correctly when install to Program Files. Please choose a difference location.";
                CanInstall = false;
                return;
            }

            /** Prohibit installing to MCC's folder
             *
             */
            if (path.Contains("Halo The Master Chief Collection"))
            {
                Status = "SPV3 does not run on MCC and it does not alter any game files within MCC." + NewLine
                       + "It is a stand alone program built on top of Halo Custom Edition.";
                CanInstall = false;
                return;
            }

            /** Check available disk space. This will NOT work on UNC paths!
             *
             */
            try
            {
                /** First, check the user's temp folder's drive to ensure there's enough free space
                  * for temporary extraction to %temp%
                  */
                {
                    var tmpath = Path.GetPathRoot(Path.GetTempPath());
                    var systemDrive = new DriveInfo(tmpath);
                    if (systemDrive.TotalFreeSpace < 11811160064)
                    {
                        Status = $"Not enough disk space (11GB required) on the {tmpath} drive. " +
                                  "Clear junk files using Disk Cleanup or allocate more space to the volume";
                        CanInstall = false;
                        return;
                    }
                }

                /**
                  * Check if the target drive has at least 16GB of free space
                  */
                var targetDrive = new DriveInfo(Path.GetPathRoot(path));

                if (targetDrive.IsReady && targetDrive.TotalFreeSpace > 17179869184)
                {
                    CanInstall = true;
                }
                else
                {
                    Status = "Not enough disk space (16GB required) at selected path: " + path;
                    CanInstall = false;
                    return;
                }
            }
            catch (Exception e)
            {
                var msg = "Failed to get drive space.\n Error:  " + e.ToString() + "\n";
                var log = (HXE.File) Paths.Exception;
                log.AppendAllText(msg);
                Status = msg;
                CanInstall = false;
            }
        }

        public void ViewActivation() // Debug widget
        {
            Main = Collapsed;
            Activation = Visible;
        }

        public void ViewMain()
        {
            Main = Visible;
            Activation = Collapsed;
        }

        public void InstallHce()
        {
            try
            {
                new Setup { Path = Path.Combine(Paths.Setup) }.Execute();
            }
            catch (Exception e)
            {
                var msg = "Failed to install Halo Custom Edition." + NewLine
                         + " Error:  " + e.ToString() + NewLine;
                var log = (HXE.File) Paths.Exception;
                var ilog = (HXE.File) Paths.Install;
                log.AppendAllText(msg);
                ilog.AppendAllText(msg);
                Status = "Failed to install Halo Custom Edition." + NewLine
                       + " Error:  " + e.Message;
            }
        }

        public void IsHaloOrCEARunning()
        {
            var inferredProcess = HXE.Process.Type.Unknown;
            try
            {
                inferredProcess = HXE.Process.Infer();
            }
            catch (Exception e)
            {
                var msg = "Failed to infer Halo process." + NewLine
                         + " Error:  " + e.ToString() + NewLine;
                var log = (HXE.File) Paths.Exception;
                var ilog = (HXE.File) Paths.Install;
                log.AppendAllText(msg);
                ilog.AppendAllText(msg);
                Status = "Failed to infer Halo process." + NewLine
                       + " Error:  " + e.Message;
            }

            if (inferredProcess != HXE.Process.Type.Unknown)
            {
                switch (inferredProcess)
                {
                    /**
                     * HPC/HCE
                     */
                    case HXE.Process.Type.Retail:
                    case HXE.Process.Type.HCE:
                        Activate("Process Detection: Halo PC/CE Found");
                        break;

                    /**
                     * MCC CEA
                     */
                    case HXE.Process.Type.Steam:
                    case HXE.Process.Type.StoreOld:
                        Activate("Process Detection: MCC CEA Found");
                        break;
                }
            }
            else
                Status = $"Process Detection: No Matching Processes{NewLine}No MCC (with CEA), HPC, or HCE processes found.";
        }

        private void Activate(string status)
        {
            Kernel.hxe.Tweaks.Patches |= Patcher.EXEP.DISABLE_DRM_AND_KEY_CHECKS;
            CanInstall = true;
            Main = Visible;
            Activation = Collapsed;
            Status = status + NewLine + _ssdRec;
        }

        public void InvokeSpv3()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = Path.Combine(Target, Paths.Executable),
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