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
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using HXE.HCE;
using HXE.Properties;
using HXE.SPV3;
using static System.Environment;
using static System.IO.Directory;
using static System.IO.Path;
using static System.Windows.Forms.Screen;
using static HXE.Console;
using static HXE.Paths;
using static HXE.HCE.Profile.ProfileAudio;
using static HXE.HCE.Profile.ProfileVideo;
using static System.Text.Encoding;

namespace HXE
{
  /// <summary>
  ///   HXE kernel version 2.
  /// </summary>
  public static class Kernel
  {
    [DllImport("USER32.DLL", EntryPoint = "SetWindowPos")]
    public static extern IntPtr SetWindowPos
    (
      IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags
    );

    [DllImport("USER32.DLL")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("USER32.DLL")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("KERNEL32.DLL")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("KERNEL32.DLL")]
    public static extern bool ReadProcessMemory
    (
      int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead
    );

    /// <summary>
    ///   Loads HCE executable in the working directory.
    /// </summary>
    public static void Invoke()
    {
      Invoke((Executable) Combine(CurrentDirectory, Paths.HCE.Executable));
    }

    /// <summary>
    ///   Loads inbound HCE executable with the default configuration.
    /// </summary>
    /// <param name="executable">
    ///   Object representing a legal HCE executable.
    /// </param>
    public static void Invoke(Executable executable)
    {
      Invoke(executable, new Configuration().Load());
    }

    /// <summary>
    ///   Loads inbound HCE executable with the inbound configuration.
    /// </summary>
    /// <param name="executable">
    ///   Object representing a legal HCE executable.
    /// </param>
    /// <param name="configuration">
    ///   Object representing the kernel configuration.
    /// </param>
    public static void Invoke(Executable executable, Configuration configuration)
    {
      /* Clear log file */
      {
        if (Exists(Paths.Exception) && new FileInfo(Paths.Exception).Length > 1048576) // If larger than 1 MiB, ...
          System.IO.File.WriteAllText(Paths.Exception, ""); // ...clear log.
      }

      /* Switch to legacy kernel modes */
      {
        if (!Exists(Legacy))
          configuration.Mode = Configuration.ConfigurationMode.SPV33;
      }

      Init(); /* initc.txt declarations */
      Blam(); /* blam.sav enhancements  */
      Open(); /* opensauce declarations */
      Exec(); /* haloce.exe invocation  */

      Core("CORE.MAIN: Successfully updated the initiation, profile and OS files, and invoked the HCE executable.");

      /**
       * We declare the contents of the initiation file in this method before dealing with the profile enhancements and
       * the executable invocation. It's the most important task, so we have to deal with it first.
       *
       * The values declared by the HXE kernel for the initiation file are categorised into the following sections:
       *
       * -   RESUME: Campaign resuming support for the SPV3 mod, which permits compatibility with SPV3's main UI;
       * -   TWEAKS: Rudimentary tweaks including cinematic bars, and controller enhancements (magnetism & auto-aim);
       * -   SHADER: SPV3 post-processing configurations, including activated shaders & shader intensity levels.
       */

      void Init()
      {
        var init = (Initiation) GetFullPath(executable.Debug.Initiation);

        Resume(); /* spv3 campaign resume with ui.map compatibility */
        Tweaks(); /* hce/spv3 start-up miscellaneous tweaks         */
        Shader(); /* spv3 post-processing toggles & settings        */
        Unlock(); /* spv3.1 legacy maps unlocking mechanism         */

        try
        {
          init.Save();

          Core("MAIN.INIT: Initiation data has been successfully applied and saved.");
        }
        catch (Exception e)
        {
          var msg = " -- MAIN.INIT HALTED.\n Error:  " + e.ToString() + "\n";
          var log = (File)Paths.Exception;
          log.AppendAllText(msg);
          Error(msg);
        }

        /**
         * Resume the campaign based on the progress of the last used profile. This requires:
         *
         * -   a LASTPROF.TXT file to be located in the profile path declared for the inbound executable; and
         * -   the directory for the profile declared in the aforementioned plaintext file; and
         * -   the savegame.bin for the respective profile, to infer the campaign progress.
         */

        void Resume()
        {
          if (configuration.Mode != Configuration.ConfigurationMode.SPV32 &&
              configuration.Mode != Configuration.ConfigurationMode.SPV33)
            return;

          try
          {
            var  lastprof = (LastProfile) Custom.LastProfile(executable.Profile.Path);

            /** Check if lastprof.txt exists
             * If it exists, load its data
             * Else, create a new one 
             */
            if (lastprof.Exists())
              lastprof.Load();
            else 
            {
              Error("Lastprof.txt does not Exist.");
              Core("Saving LastProf.txt...");
              lastprof.Save();
            }

            var name = lastprof.Profile;
            var save = (Progress) Custom.Progress(executable.Profile.Path, name);

            /** Check for the existence of the blam.sav specified by LastProfile.
             * If it exists, skip this abomination.
             * Else, create a new player profile
             *   or search for existing profiles.
             */
            if (!save.Exists())
            {
              Error("Player Profile specified in LastProfile not found.");
              var savegames = Custom.Profiles(executable.Profile.Path);

              /** Check if the Savegames (profiles) folder exists.
               * If it exists, search for existing profiles.
               * Else, create a new player profile.
               */
              if (Exists(savegames))
              {
                List<Profile> profiles      = Profile.List(savegames);
                List<Profile> validProfiles = new List<Profile>{ };

                /** Check for any existing profile folders 
                 * If any are found, check if their blam.sav also exists.
                 *  Select the first valid profile.
                 * Else, create a new player profile.
                 */
                if (profiles.Count != 0)
                {
                  foreach (Profile profile in profiles)
                  {
                    if (profile.Exists())
                      validProfiles.Add(profile); // todo: implement better validation
                  }
                  lastprof.Profile = validProfiles[0].Details.Name;
                  lastprof.Save();
                }
                else
                {
                  Error("No profiles found in savegames folder.");
                  Core("Generating new profile");
                  NewProfile.Generate(executable.Profile.Path, lastprof, new Profile(), false);
                }
              }
              else
              {
                Error("Savegames folder does not exist.");
                Core("Creating savegames folder and a new profile...");
                NewProfile.Generate(executable.Profile.Path, lastprof, new Profile(), false);;
              }
              name = lastprof.Profile;
              save = (Progress) Custom.Progress(executable.Profile.Path, name);
            }

            var campaign = new Campaign(Paths.Campaign(configuration.Mode));

            campaign.Load();
            save.Load(campaign);

            init.Progress = save;
            init.Resume = campaign.Resume;

            Core("INIT.RESUME: Campaign checkpoint information has been applied to the initiation file.");

            Debug("INIT.RESUME: Mission    - " + init.Progress.Mission.Name);
            Debug("INIT.RESUME: Difficulty - " + init.Progress.Difficulty.Name);
          }
          catch (Exception e)
          {
            var msg = " -- INIT.RESUME HALTED\n Error:  " + e.ToString() + "\n";
            var log = (File) Paths.Exception;
            log.AppendAllText(msg);
            Error(e.Message + " -- INIT.RESUME HALTED");
          }
        }

        /**
         * Rudimentary tweaks including:
         *
         * -   Player Auto-aim
         * -   Player Magnetism
         * -   Acceleration
         *
         * ... and stuff for SPV3:
         *
         * -   Cinematic Bars
         * -   Motion Sensor
         */

        void Tweaks()
        {
          init.PlayerAutoaim     = configuration.Tweaks.AutoAim;
          init.PlayerMagnetism   = configuration.Tweaks.Magnetism;
          init.MouseAcceleration = configuration.Tweaks.Acceleration;
          init.Gamma             = configuration.Video.Gamma;

          if (configuration.Mode == Configuration.ConfigurationMode.SPV32 ||
              configuration.Mode == Configuration.ConfigurationMode.SPV33)
          {
            init.CinemaBars    = configuration.Tweaks.CinemaBars;
            init.MotionSensor  = configuration.Tweaks.Sensor;
            init.Unload        = configuration.Tweaks.Unload;

            Core("INIT.TWEAKS: SPV3 cinematic bars, motion sensor and unload tweaks have been updated.");

            Debug("INIT.TWEAKS: Cinematic Bars - " + init.CinemaBars);
            Debug("INIT.TWEAKS: Motion Sensor  - " + init.MotionSensor);
            Debug("INIT.TWEAKS: Unload         - " + init.Unload);
          }

          Core("INIT.TWEAKS: Magnetism, auto-aim, and mouse acceleration tweaks have been updated.");

          Debug("INIT.TWEAKS: Player Auto-aim    - " + init.PlayerAutoaim);
          Debug("INIT.TWEAKS: Player Magnetism   - " + init.PlayerMagnetism);
          Debug("INIT.TWEAKS: Mouse Acceleration - " + init.MouseAcceleration);
          Debug("INIT.TWEAKS: Gamma              - " + init.Gamma);
        }

        /**
         * SPV3 shader toggles and configurations.
         */

        void Shader()
        {
          if (configuration.Mode != Configuration.ConfigurationMode.SPV32 &&
              configuration.Mode != Configuration.ConfigurationMode.SPV33)
            return;

          init.Shaders = configuration.Shaders;

          Core("INIT.SHADER: SPV3 post-processing effects have been assigned to the initiation file.");
        }

        /**
         * Indeed, SPV3.1 had some DRM I wasn't quite a fan of. This routine unlocks the 3.1 campaigns. Is this a crack?
         * I should have a demo screen playing here, along with greetings to friends all over the world.
         *
         * Idea: Release 3.2 with an NFO file with ANSI art.
         */

        void Unlock()
        {
          if (configuration.Mode != Configuration.ConfigurationMode.SPV32)
            return;

          init.Unlock = true;

          Core("INIT.UNLOCK: SPV3.1 campaign has been unlocked for subsequent invocation.");
        }
      }

      /**
       * We enhance the player's profile by applying the highest video & audio quality settings, along with forcing the
       * resolution declared in video parameters of the inbound executable.
       *
       * The enhancements are applied based on the provided configuration and the successful inference
       * the last used profile in the specified profiles directory.
       */

      void Blam()
      {
        Profile blam;

        try
        {
          blam = Profile.Detect(executable.Profile.Path);
             
          Video();
          Audio();
          Input();

          blam.Save();

          Core("MAIN.BLAM: Profile enhancements have been successfully applied and saved.");
        }
        catch (FileNotFoundException)
        {
          var lastprof = (LastProfile) Custom.LastProfile(executable.Profile.Path);
          var scaffold = lastprof.Exists() && System.IO.File.Exists(Custom.Profile(executable.Profile.Path, lastprof.Profile));

          if (!lastprof.Exists())
            Core("Lastprof.txt does not exist.");

          if (!scaffold)
            Debug("Savegames scaffold doesn't exist.");
          else
            Debug("Savegames scaffold detected.");

          Core("Calling LastProfile.Generate()...");
          NewProfile.Generate(executable.Profile.Path, lastprof, blam = new Profile(), scaffold);
        }
        catch (Exception e)
        {
          var msg = " -- MAIN.BLAM HALTED\n Error:  " + e.ToString() + "\n";
          var log = (File)Paths.Exception;
          log.AppendAllText(msg);
          Error(msg);
        }

        /**
         * Apply the resolution specified in video parameters of the inbound executable OR apply the primary screen's
         * current resolution. This enforces HCE to run at the desired or native resolution, and also obsoletes Ecran.
         *
         * Additionally, effects and qualities are enabled and set to the maximum level, respectively. Of course, this
         * also depends on the provided configuration.
         */

        void Video()
        {
          if (!configuration.Video.ResolutionEnabled)
          {
            // infer from resolution if Native Resoluton preferred.
            if (executable.Video.Width == 0 || executable.Video.Height == 0)
            {
              executable.Video.Width  = (ushort) PrimaryScreen.Bounds.Width;
              executable.Video.Height = (ushort) PrimaryScreen.Bounds.Height;

              Core("BLAM.VIDEO.RESOLUTION: No resolution provided. Applied native resolution to executable.");
            }
            else if (executable.Video.Width  > (ushort) PrimaryScreen.Bounds.Width ||
                     executable.Video.Height > (ushort) PrimaryScreen.Bounds.Height)
            {
              executable.Video.Width  = (ushort) PrimaryScreen.Bounds.Width;
              executable.Video.Height = (ushort) PrimaryScreen.Bounds.Height;

              Core("BLAM.VIDEO.RESOLUTION: Resolution out of bounds. Applied native resolution to executable.");
            }

            blam.Video.Resolution.Width  = executable.Video.Width;
            blam.Video.Resolution.Height = executable.Video.Height;

            Core("BLAM.VIDEO.RESOLUTION: Executable resolution has been assigned to the inferred profile.");

            Debug("BLAM.VIDEO.RESOLUTION: Width  - " + blam.Video.Resolution.Width);
            Debug("BLAM.VIDEO.RESOLUTION: Height - " + blam.Video.Resolution.Height);
          }

          if (configuration.Video.Bless)
          {
            executable.Video.Width  -= 10;
            executable.Video.Height -= 10;
            Core("BLAM.VIDEO.BLESS: Tweaked executable resolution for border-less support.");
          }

          if (configuration.Video.Quality)
          {
            blam.Video.Effects.Specular = true;
            blam.Video.Effects.Shadows  = true;
            blam.Video.Effects.Decals   = true;

            blam.Video.Quality   = VideoQuality.High;
            blam.Video.Particles = VideoParticles.High;

            Core("BLAM.VIDEO.QUALITY: Patched effects and quality to the highest levels.");

            Debug("BLAM.VIDEO.QUALITY: Specular  - " + blam.Video.Effects.Specular);
            Debug("BLAM.VIDEO.QUALITY: Shadows   - " + blam.Video.Effects.Shadows);
            Debug("BLAM.VIDEO.QUALITY: Decals    - " + blam.Video.Effects.Decals);
            Debug("BLAM.VIDEO.QUALITY: Quality   - " + blam.Video.Quality);
            Debug("BLAM.VIDEO.QUALITY: Particles - " + blam.Video.Particles);
          }

          if (configuration.Video.Uncap)
          {
            blam.Video.FrameRate = VideoFrameRate.VsyncOff; 

            Core("BLAM.VIDEO.UNCAP: Applied V-Sync off for framerate uncap.");

            Debug("BLAM.VIDEO.UNCAP: Framerate - " + blam.Video.FrameRate);
          }

          Core("BLAM.VIDEO: Video enhancements have been applied accordingly.");
        }

        /**
         * Apply the highest audio settings and toggle Hardware Acceleration & EAX. The latter will require a compatible
         * library such as DSOAL for it to actually work.
         */

        void Audio()
        {
          if (configuration.Audio.Quality)
          {
            blam.Audio.Quality = AudioQuality.High;
            blam.Audio.Variety = AudioVariety.High;

            Core("BLAM.AUDIO.QUALITY: Patched audio quality to the highest level.");

            Debug("BLAM.AUDIO.QUALITY: Quality - " + blam.Audio.Quality);
            Debug("BLAM.AUDIO.QUALITY: Variety - " + blam.Audio.Variety);
          }

          if (configuration.Audio.Enhancements)
          {
            if (System.IO.File.Exists(DSOAL))
            {
              blam.Audio.HWA = true;
              blam.Audio.EAX = true;

              Core("BLAM.AUDIO.ENHANCEMENTS: Enabled Hardware Acceleration & EAX.");
            }
            else
            {
              blam.Audio.HWA = false;
              blam.Audio.EAX = false;

              Core("BLAM.AUDIO.ENHANCEMENTS: DSOAL not found. Refusing to enable HWA & EAX..");
            }

            Debug("BLAM.AUDIO.ENHANCEMENTS: HWA - " + blam.Audio.HWA);
            Debug("BLAM.AUDIO.ENHANCEMENTS: EAX - " + blam.Audio.EAX);
          }

          Core("BLAM.AUDIO: Audio enhancements have been applied accordingly.");
        }

        /**
         * Apply SPV3's preset input to the controller.
         */

        void Input()
        {
          if (!configuration.Input.Override)
            return;

          blam.Input.Mapping = new Dictionary<Profile.ProfileInput.Action, Profile.ProfileInput.Button>
          {
            {Profile.ProfileInput.Action.MoveForward, Profile.ProfileInput.Button.LSU},
            {Profile.ProfileInput.Action.MoveBackward, Profile.ProfileInput.Button.LSD},
            {Profile.ProfileInput.Action.MoveLeft, Profile.ProfileInput.Button.LSL},
            {Profile.ProfileInput.Action.MoveRight, Profile.ProfileInput.Button.LSR},
            {Profile.ProfileInput.Action.Crouch, Profile.ProfileInput.Button.LSM},
            {Profile.ProfileInput.Action.Reload, Profile.ProfileInput.Button.DPU},
            {Profile.ProfileInput.Action.Jump, Profile.ProfileInput.Button.A},
            {Profile.ProfileInput.Action.SwitchGrenade, Profile.ProfileInput.Button.B},
            {Profile.ProfileInput.Action.Action, Profile.ProfileInput.Button.X},
            {Profile.ProfileInput.Action.SwitchWeapon, Profile.ProfileInput.Button.Y},
            {Profile.ProfileInput.Action.LookUp, Profile.ProfileInput.Button.RSU},
            {Profile.ProfileInput.Action.LookDown, Profile.ProfileInput.Button.RSD},
            {Profile.ProfileInput.Action.LookLeft, Profile.ProfileInput.Button.RSL},
            {Profile.ProfileInput.Action.LookRight, Profile.ProfileInput.Button.RSR},
            {Profile.ProfileInput.Action.ScopeZoom, Profile.ProfileInput.Button.RSM},
            {Profile.ProfileInput.Action.ThrowGrenade, Profile.ProfileInput.Button.LB},
            {Profile.ProfileInput.Action.Flashlight, Profile.ProfileInput.Button.LT},
            {Profile.ProfileInput.Action.MeleeAttack, Profile.ProfileInput.Button.RB},
            {Profile.ProfileInput.Action.FireWeapon, Profile.ProfileInput.Button.RT}
          };

          blam.Input.BitBinding = new Dictionary<Profile.ProfileInput.OddValues, Profile.ProfileInput.OddOffsets>
          {
            {Profile.ProfileInput.OddValues.Button1, Profile.ProfileInput.OddOffsets.MenuAccept},
            {Profile.ProfileInput.OddValues.Button2, Profile.ProfileInput.OddOffsets.MenuBack}
          };

          Core("BLAM.INPUT: Input overrides have been applied accordingly.");
        }
      }

      /**
       * For SPV3.2+, we will tweak the OpenSauce settings to ensure full compatibility with 3.2+'s specifications and
       * post-processing effects.
       */

      void Open()
      {
        var open = (OpenSauce) Custom.OpenSauce(executable.Profile.Path);
        var mod = System.IO.File.Exists("./dinput8.dll") ||
                  System.IO.File.Exists("./mods/opensauce.dll");

        if (System.IO.File.Exists("./dinput8.dll"))
          Debug("dinput8.dll exists");
        else
          Debug("dinput8 not found");

        if (System.IO.File.Exists("./mods/opensauce.dll"))
          Debug("opensauce.dll exists");
        else
          Debug("opensauce.dll not found");

        if (!mod)
          Debug("Open Sauce not found");

        if (open.Exists() && mod)
        { 
          open.Load(); 
        }
        else if(mod)
        {
          open.Save();
          open.Load();
        }
        

        try
        {
          if (configuration.Mode == Configuration.ConfigurationMode.HCE) return;

          open.Rasterizer.PostProcessing.MotionBlur.Enabled = (configuration.Shaders & PP.MOTION_BLUR_BUILT_IN) != 0;
          open.HUD.ScaleHUD                                 = true; /* fixes user interface    */
          open.Camera.IgnoreFOVChangeInCinematics           = true; /* fixes user interface    */
          open.Camera.IgnoreFOVChangeInMainMenu             = true; /* fixes user interface    */
          open.Rasterizer.ShaderExtensions.Effect.DepthFade = true; /* shader optimisations    */ 

          open.Save();

          Core("MAIN.OPEN: Conditionally applied SPV3 fixes - enabled HUD scaling, FOV ignoreing, and depth fade.");

          Debug("MAIN.OPEN: Motion Blur          - " + open.Rasterizer.PostProcessing.MotionBlur.Enabled);
          Debug("MAIN.OPEN: HUD Scaling          - " + open.HUD.ScaleHUD);
          Debug("MAIN.OPEN: Cinematic FOV Ignore - " + open.Camera.IgnoreFOVChangeInCinematics);
          Debug("MAIN.OPEN: Main Menu FOV Ignore - " + open.Camera.IgnoreFOVChangeInMainMenu);
          Debug("MAIN.OPEN: Depth Fade Effects   - " + open.Rasterizer.ShaderExtensions.Effect.DepthFade);
        }
        catch (Exception e)
        {
          var msg = " -- MAIN.OPEN\n Error:  " + e.ToString() + "\n";
          var log = (File)Paths.Exception;
          log.AppendAllText(msg);
          Error(msg);
        }
      }

      /**
       * Gracefully halt any potentially hanging HCE processes, conditionally patch the HCE executable with LAA flag,
       * and start the executable.
       */

      void Exec()
      {
        Reset();
        Patch();
        Start();
        Bless();

        Core("MAIN.EXEC: All HCE execution routines have been successfully resolved.");

        /**
         * This method encourages the kernel to wait for a potential HCE process to end before proceeding with any
         * subsequent routines. For some odd reason, there are cases where HCE runs in the background after the end-user
         * exits it, thus prohibiting any additional processes from being invoked. To mitigate that, we gracefully kill
         * any existing ones.
         */

        void Reset()
        {
          Info("Killing existing HCE processes");

          try
          {
            foreach (var process in Process.GetProcessesByName("haloce"))
              process.Kill();
          }
          catch (Exception e)
          {
            var msg = " -- MAIN.BLAM HALTED\n Error:  " + e.ToString() + "\n";
            var log = (File)Paths.Exception;
            log.AppendAllText(msg);
            Info(msg);
          }

          var tries = 0;
          Wait("Waiting for existing HCE process to end ");

          while (Process.GetProcessesByName("haloce").Length > 0 && tries <= 25)
          {
            System.Console.Write(Resources.Progress);
            tries++;
          }

          if (tries == 25)
            Error("Could not kill HCE process. Process initiation errors may occur.");

          Core("EXEC.RESET: Finalised attempts of killing potential HCE processes.");
        }

        /**
         * If the HCE executable is not already patched with the LAA flag, this method will take care of modifying the
         * byte's value to enable the respective LAA flag.
         */

        void Patch()
        {
          if (configuration.Tweaks.Acceleration)
            configuration.Tweaks.Patches |= Patcher.EXEP.DISABLE_MOUSE_ACCELERATION;
          if (configuration.Video.GammaOn == false)
            configuration.Tweaks.Patches |= Patcher.EXEP.DISABLE_SYSTEM_GAMMA;
          
          try
          {
            new Patcher().Write(configuration.Tweaks.Patches, executable.Path);
            Core("EXEC.PATCH: Conditional patching has been handled.");
          }
          catch (Exception e)
          {
            var msg = " -- EXEC.PATCH HALTED\n Error:  " + e.ToString() + "\n";
            var log = (File)Paths.Exception;
            log.AppendAllText(msg);
            Error(msg);
          }
        }

        /**
         * God is on my side if this method actually gets called.
         */

        void Start()
        {
          try
          {
            executable.Start(configuration.Main.Elevated);

            Core("EXEC.START: Successfully started the inferred HCE executable.");
          }
          catch (Exception e)
          {
            var msg = " -- EXEC.START HALTED\n Error:  " + e.ToString() + "\n";
            var log = (File)Paths.Exception;
            log.AppendAllText(msg);
            Error(msg);
          }
        }

        /**
         * Bless for border-less!
         */

        void Bless()
        {
          if (!configuration.Video.Bless)
            return;

          const int LOADED_OFFSET   = 0x0064533B; /* 0x6B4051 could also be used, according to github.com/gbMichelle */
          const int PROCESS_WM_READ = 0x0010;
          const int GWL_STYLE       = -16;
          const int SWP_NOMOVE      = 0X2;
          const int SWP_NOSIZE      = 1;
          const int SWP_NOZORDER    = 0X4;
          const int SWP_SHOWWINDOW  = 0x0040;
          const int WS_SYSMENU      = 0x00080000;
          const int WS_MAXIMIZEBOX  = 0x00010000;
          const int WS_MINIMIZEBOX  = 0x00020000;
          const int WS_SIZEBOX      = 0x00040000;
          const int WS_BORDER       = 0x00800000;
          const int WS_DLGFRAME     = 0x00400000;
          const int WS_CAPTION =
            SWP_NOMOVE | WS_SYSMENU | WS_MAXIMIZEBOX | WS_MINIMIZEBOX | WS_BORDER | WS_DLGFRAME | WS_SIZEBOX;

          try
          {
            var processes = Process.GetProcesses();

            foreach (var process in processes)
            {
              if (!process.ProcessName.StartsWith("haloce"))
                continue;

              /**
               * The bless hack is applied only when HCE has been loaded. HXE deems HCE as loaded when the main menu has
               * been initiated.
               *
               * This is determined by checking the value declared at LOADED_OFFSET. The value changes from 0 to 1 the
               * moment HCE has loaded.
               *
               * The reason we apply the hack then is for robustness: the HCE window becomes responsive around that time
               * after the loading sequence.
               */

              var processHandle = OpenProcess(PROCESS_WM_READ, false, process.Id);

              var bytesRead = 0;
              var buffer    = new byte[1];
              var loaded    = false;

              while (!loaded && process.HasExited != true)
              {
                ReadProcessMemory((int) processHandle, LOADED_OFFSET, buffer, buffer.Length, ref bytesRead);
                loaded = buffer[0] == 1;
              }

              /**
               * The hack attempts to hide any window controls and also move the window to the top left of the current
               * display. In most circumstances, this should work just fine. If people are applying border-less mode and
               * use a resolution lower than their screen resolution, then, err... why?
               */

              var pFoundWindow = process.MainWindowHandle;
              var style        = GetWindowLong(pFoundWindow, GWL_STYLE);
              SetWindowLong(pFoundWindow, GWL_STYLE, style & ~WS_CAPTION);
              SetWindowPos(pFoundWindow, 0, 0, 0, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
              Core("EXEC.BLESS: Applied border-less hack to the HCE process window.");
            }
          }
          catch (Exception e)
          {
            var msg = " -- EXEC.START HALTED\n Error:  " + e.ToString() + "\n";
            var log = (File)Paths.Exception;
            log.AppendAllText(msg);
            Error(msg);
          }
        }
      }
    }

    /// <summary>
    ///   HXE kernel configuration version 2.
    /// </summary>
    public class Configuration
    {
      public enum ConfigurationMode
      {
        HCE,   /* tweaks, hce patches & enhancements */
        SPV32, /* shaders, campaign resume, tweaks   */
        SPV33  /* SPV32, campaign++, deband          */
      }

      private const    int    Length = 256; /* persistence binary length */
      private readonly string _path;        /* persistence binary path   */

      public Configuration()
      {
        _path = Paths.Configuration;
      }

      public Configuration(string path)
      {
        _path = path;
      }

      public ConfigurationMode   Mode    { get; set; } = ConfigurationMode.HCE;     /* kernel mode        */
      public ConfigurationMain   Main    { get; set; } = new ConfigurationMain();   /* main invocations   */
      public ConfigurationVideo  Video   { get; set; } = new ConfigurationVideo();  /* profile video      */
      public ConfigurationAudio  Audio   { get; set; } = new ConfigurationAudio();  /* profile audio      */
      public ConfigurationInput  Input   { get; set; } = new ConfigurationInput();  /* profile input      */
      public ConfigurationTweaks Tweaks  { get; set; } = new ConfigurationTweaks(); /* profile tweaks     */
      public uint                Shaders { get; set; } = 0;                         /* spv3 shaders       */
      public string              Path    { get => _path; }

      /// <summary>
      ///   Persists object state to the filesystem.
      /// </summary>
      /// <remarks>
      ///   When the offsets of the file changes,
      ///   increment the config version.
      /// </remarks>
      public Configuration Save()
      {
        CreateDirectory(GetDirectoryName(_path)
                        ?? throw new ArgumentException("Configuration directory path is null."));

        using (var fs = new FileStream(_path, FileMode.Create, FileAccess.Write))
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
            ms.Position = (byte) Offset.Signature;
            bw.Write(Unicode.GetBytes("~yumiris"));
          }

          /* mode */
          {
            ms.Position = (byte) Offset.Mode;
            bw.Write((byte) Mode);
          }

          /* main */
          {
            ms.Position = (byte) Offset.Main;
            bw.Write(Main.Reset);
            bw.Write(Main.Patch);
            bw.Write(Main.Start);
            bw.Write(Main.Resume);
            bw.Write(Main.Elevated);
          }

          /* video */
          {
            ms.Position = (byte) Offset.Video;
            bw.Write(Video.ResolutionEnabled);
            bw.Write(Video.Uncap);
            bw.Write(Video.Quality);
            bw.Write(Video.GammaOn);
            bw.Write(Video.Gamma);
            bw.Write(Video.Bless);
          }

          /* audio */
          {
            ms.Position = (byte) Offset.Audio;
            bw.Write(Audio.Quality);
            bw.Write(Audio.Enhancements);
          }

          /* input */
          {
            ms.Position = (byte) Offset.Input;
            bw.Write(Input.Override);
          }

          /* tweaks */
          {
            ms.Position = (byte) Offset.Tweaks;
            bw.Write(Tweaks.CinemaBars);
            bw.Write(Tweaks.Sensor);
            bw.Write(Tweaks.Magnetism);
            bw.Write(Tweaks.AutoAim);
            bw.Write(Tweaks.Acceleration);
            bw.Write(Tweaks.Unload);
            bw.Write(Tweaks.Patches);
          }

          /* shaders */
          {
            ms.Position = (byte) Offset.Shaders;
            bw.Write(Shaders);
          }

          /* persist */
          {
            ms.Position = 0;
            ms.CopyTo(fs);
          }
        }

        return this;
      }

      /// <summary>
      ///   Loads object state from the filesystem.
      /// </summary>
      public Configuration Load()
      {
        if (!System.IO.File.Exists(_path))
          return this;

        using (var fs = new FileStream(_path, FileMode.Open, FileAccess.Read))
        using (var ms = new MemoryStream(Length))
        using (var br = new BinaryReader(ms))
        {
          /* loading */
          {
            fs.CopyTo(ms);
            ms.Position = 0;
          }

          /* mode */
          {
            ms.Position = (byte) Offset.Mode;
            Mode        = (ConfigurationMode) br.ReadByte();
          }

          /* main */
          {
            ms.Position   = (byte) Offset.Main;
            Main.Reset    = br.ReadBoolean();
            Main.Patch    = br.ReadBoolean();
            Main.Start    = br.ReadBoolean();
            Main.Resume   = br.ReadBoolean();
            Main.Elevated = br.ReadBoolean();
          }

          /* video */
          {
            ms.Position             = (byte) Offset.Video;
            Video.ResolutionEnabled = br.ReadBoolean();
            Video.Uncap             = br.ReadBoolean();
            Video.Quality           = br.ReadBoolean();
            Video.GammaOn           = br.ReadBoolean();
            Video.Gamma             = br.ReadByte();
            Video.Bless             = br.ReadBoolean();
          }

          /* audio */
          {
            ms.Position        = (byte) Offset.Audio;
            Audio.Quality      = br.ReadBoolean();
            Audio.Enhancements = br.ReadBoolean();
          }

          /* input */
          {
            ms.Position    = (byte) Offset.Input;
            Input.Override = br.ReadBoolean();
          }

          /* tweaks */
          {
            ms.Position         = (byte) Offset.Tweaks;
            Tweaks.CinemaBars   = br.ReadBoolean();
            Tweaks.Sensor       = br.ReadBoolean();
            Tweaks.Magnetism    = br.ReadBoolean();
            Tweaks.AutoAim      = br.ReadBoolean();
            Tweaks.Acceleration = br.ReadBoolean();
            Tweaks.Unload       = br.ReadBoolean();
            Tweaks.Patches      = br.ReadUInt32();
          }

          /* shaders */
          {
            ms.Position                = (byte) Offset.Shaders;
            Shaders                    = br.ReadUInt32();
          }
        }

        return this;
      }

      private enum Offset
      {
        Start     = 0x00,
        Signature = Start     + 0x00,
        Version   = Signature + 0x10,
        Mode      = Version   + 0x10,
        Main      = Mode      + 0x10,
        Video     = Main      + 0x10,
        Audio     = Video     + 0x10,
        Input     = Audio     + 0x10,
        Tweaks    = Input     + 0x10,
        Shaders   = Tweaks    + 0x40
      }

      public class ConfigurationMain
      {
        public bool Reset    { get; set; } = true;  /* kill HCE process   */
        public bool Patch    { get; set; } = true;  /* patch LAA flag     */
        public bool Start    { get; set; } = true;  /* invoke HCE process */
        public bool Resume   { get; set; } = true;  /* resume mission     */
        public bool Elevated { get; set; } = false; /* invoke as admin   */
      }

      public class ConfigurationVideo
      {
        public bool ResolutionEnabled { get; set; } = false; /* custom resolution */
        public bool Uncap             { get; set; } = true;  /* unlock framerate   */
        public bool Quality           { get; set; }          /* set to false by default for optimisation */
        public bool GammaOn           { get; set; } = false; /* enable hce gamma   */
        public byte Gamma             { get; set; }          /* game video gamma   */
        public bool Bless             { get; set; } = false; /* border-less hack   */
      }

      public class ConfigurationAudio
      {
        public bool Quality      { get; set; } = true; /* auto high quality */
        public bool Enhancements { get; set; } = true; /* eax/hardware acc. */
      }

      public class ConfigurationInput
      {
        public bool Override { get; set; } = true; /* override mapping */
      }

      public class ConfigurationTweaks
      {
        public bool   CinemaBars   { get; set; } = true; /* SPV3 cinematic bars   */
        public bool   Sensor       { get; set; } = true; /* SPV3 motion sensor    */
        public bool   Magnetism    { get; set; } = true; /* controller magnetism  */
        public bool   AutoAim      { get; set; } = true; /* controller auto-aim   */
        public bool   Acceleration { get; set; }         /* mouse acceleration    */
        public bool   Unload       { get; set; }         /* unload SPV3 shaders   */
        public uint   Patches      { get; set; } = 0;    /* haloce exe patches    */ /** See HXE.Patches.KPatches */
      }
    }
  }
}