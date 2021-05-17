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
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using HXE;
using HXE.HCE;
using HXE.SPV3;
using SPV3.Annotations;
using File = System.IO.File;
using Loader = SPV3.Configuration.ConfigurationLoader;

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
                var openSauce = (OpenSauce) HXE.Paths.Custom.OpenSauce(Paths.Directory); /** for menu fixes, gfx, modes */
                var chimera = (Chimera) HXE.Paths.Custom.Chimera(Paths.Directory);     /** for interpolation          */

                /** Load HXE kernel & SPV3 Loader instances.
                 * Overrides moved to Kernel class.
                 * Assign SPV3 values to HXE equivalents.
                 */
                Kernel.Load();

                /** Switch to Legacy SPV3 mode */
                if (!File.Exists(HXE.Paths.Legacy))
                {
                    Kernel.hxe.Mode = HXE.Kernel.Configuration.ConfigurationMode.SPV33;
                }

                /** Load Chimera binary file */
                if (chimera.Exists())
                {
                    chimera.Load();
                }
                else
                {
                    chimera.Interpolation = 8;
                    chimera.AnisotropicFiltering = false;
                    chimera.UncapCinematic = true;
                    chimera.BlockLOD = false;
                }

                /** Load OpenSauce XML file */
                if (openSauce.Exists())
                    openSauce.Load();
                else
                    openSauce.Camera.CalculateFOV();

                if (!File.Exists(Paths.Kernel))
                    openSauce.Camera.CalculateFOV();

                openSauce.HUD.ShowHUD = true;
                openSauce.Rasterizer.PostProcessing.MapEffects.Enabled = false; /* for opensauce to interpret as true  */
                openSauce.Rasterizer.PostProcessing.ExternalEffects.Enabled = true;  /* for opensauce to interpret as false */

                if (Kernel.spv3.DOOM && !Kernel.spv3.Photo)
                    if (File.Exists(Paths.DOOM))
                    {
                        openSauce.Objects.Weapon.Load(Paths.DOOM);
                        openSauce.HUD.ShowHUD = true;
                    }

                if (Kernel.spv3.Photo && !Kernel.spv3.DOOM)
                    if (File.Exists(Paths.Photo))
                    {
                        openSauce.Objects.Weapon.Load(Paths.Photo);
                        openSauce.HUD.ShowHUD = false;
                        Kernel.hxe.Tweaks.Sensor = false;
                    }

                if (openSauce.Camera.FieldOfView < 40.00 || openSauce.Camera.FieldOfView > 180.00)
                    openSauce.Camera.CalculateFOV();

                Kernel.Save();      /* saves to %APPDATA%\SPV3\kernel-0x##.bin, loader-0x##.bin */
                chimera.Save();     /* saves to %APPDATA%\SPV3\chimera.bin                      */
                openSauce.Save();   /* saves to %APPDATA%\SPV3\OpenSauce\OS_Settings.User.xml   */

                HXE.Kernel.Invoke(new Executable
                {
                    Path = Path.Combine(Environment.CurrentDirectory, HXE.Paths.HCE.Executable),
                    Profile = new Executable.ProfileOptions
                    {
                        Path = SPV3.Paths.Directory
                    },
                    Video = new Executable.VideoOptions
                    {
                        DisplayMode = Kernel.spv3.Vsync,
                        Width = Kernel.spv3.ResolutionEnabled ? Kernel.spv3.Width : (ushort) 0,
                        Height = Kernel.spv3.ResolutionEnabled ? Kernel.spv3.Height : (ushort) 0,
                        Refresh = Kernel.spv3.Vsync ? Kernel.spv3.Framerate : (ushort) 0,
                        Window = Kernel.spv3.DisplayMode != (byte) Loader.DisplayModes.Fullscreen,
                        Adapter = (byte) (Kernel.spv3.Adapter + 1),
                        NoGamma = Kernel.hxe.Video.GammaOn == false
                    },
                    Debug = new Executable.DebugOptions
                    {
                        Console = true,
                        Developer = true,
                        Screenshot = true,
                        Initiation = Paths.Initiation
                    },
                    Miscellaneous = new Executable.MiscellaneousOptions
                    {
                        NoVideo = true
                    }
                }, Kernel.hxe);
            }

            [NotifyPropertyChangedInvocator]
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}