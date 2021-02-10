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

using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.File;
using static System.IO.Path;
using static System.Diagnostics.Process;

namespace HXE
{
  /// <summary>
  ///   Lists all of the files & directories on the filesystem that HCE/HXE deals with.
  /// </summary>
  public static class Paths
  {
    public const string Executable = "hxe.exe";
    public const string Manifest   = "manifest.bin";

    public static readonly string StartDirectory= Combine(GetDirectoryName(GetCurrentProcess().MainModule.FileName));
    public static readonly string Directory     = Combine(GetFolderPath(ApplicationData), "HXE");
    public static readonly string Configuration = Combine(Directory,                      "kernel-0x05.bin");
    public static readonly string Exception     = Combine(Directory,                      "exception.log");
    public static readonly string Positions     = Combine(CurrentDirectory,               "positions.bin");
    public static readonly string DSOAL         = Combine(CurrentDirectory,               "dsoal-aldrv.dll");
    public static readonly string DSOUND        = Combine(CurrentDirectory,               "dsound.dll");
    public static readonly string ALSoft        = Combine(CurrentDirectory,               "alsoft.ini");
    public static readonly string Legacy        = Combine(CurrentDirectory,               "legacy.txt");

    public static string Campaign(Kernel.Configuration.ConfigurationMode mode)
    {
      return Combine(CurrentDirectory, $"{mode.ToString().ToLower()}.xml");
    }

    public class HCE
    {
      public const           string Executable = "haloce.exe";
      public static readonly string Directory  = Combine(GetFolderPath(Personal), "My Games", "Halo CE");

      public static readonly string Profiles    = Combine(Directory, "savegames");
      public static readonly string LastProfile = Combine(Directory, "lastprof.txt");
      public static readonly string Chimera     = Combine(Directory, "chimera.bin");
      public static readonly string OpenSauce   = Combine(Directory, "OpenSauce", "OS_Settings.User.xml");

      /*
       * The initiation filename is declared based on the presence of the OpenSauce DLL in the current directory or mods subdirectory.
       * 
       * -   initc is interpreted by OpenSauce (InitC = Initiation Client file)
       * -   init is primarily used by dedicated HCE servers for startup commands
       *
       * See #20 on github:SPV3
       */
      public static readonly string Initiation
        = Exists("dinput8.dll") || Exists("mods/opensauce.dll")
          ? Combine(CurrentDirectory, "initc.txt")
          : Combine(CurrentDirectory, "init.txt");

      public static string ProfileDirectory(string profile)
      {
        return Combine(Directory, Profiles, profile);
      }

      public static string Profile(string profile)
      {
        return Combine(Directory, Profiles, profile, "blam.sav");
      }

      public static string Progress(string profile)
      {
        return Combine(Directory, Profiles, profile, "savegame.bin");
      }

      public static string Waypoint(string profile)
      {
        return Combine(Directory, Profiles, profile, profile);
      }
    }

    public class Custom
    {
      public static string Profiles(string directory)
      {
        return Combine(directory, "savegames");
      }

      public static string LastProfile(string directory)
      {
        return Combine(directory, "lastprof.txt");
      }

      public static string Chimera(string directory)
      {
        return Combine(directory, "chimera.bin");
      }

      public static string OpenSauce(string directory)
      {
        return Combine(directory, "OpenSauce", "OS_Settings.User.xml");
      }

      /// <summary>
      /// Provide the -path parameter's string and a Player Profile name to get the path of its directory.
      /// </summary>
      /// <param name="directory">The path provided by the -path executable parameter.</param>
      /// <param name="profile">The name of the provided Player Profile.</param>
      /// <returns>The path of the Player Profile's directory.</returns>
      public static string ProfileDirectory(string directory, string profile)
      {
        return Combine(Profiles(directory), profile);
      }

      /// <summary>
      /// Provide the -path parameter's string and a Player Profile name to get the path of its blam.sav file.
      /// </summary>
      /// <param name="directory">The path provided by the -path executable parameter.</param>
      /// <param name="profile">The name of the provided Player Profile.</param>
      /// <returns>The path to the Player Profile's blam.sav file.</returns>
      public static string Profile(string directory, string profile)
      {
        return Combine(Profiles(directory), profile, "blam.sav");
      }

      /// <summary>
      /// Provide the -path parameter's string and a Player Profile name to get the path of its savegame.bin file.
      /// </summary>
      /// <param name="directory">The path provided by the -path executable parameter.</param>
      /// <param name="profile">The name of the provided Player Profile.</param>
      /// <returns>The path to the Player Profile's savegame.bin file.</returns>
      public static string Progress(string directory, string profile)
      {
        return Combine(Profiles(directory), profile, "savegame.bin");
      }

      /// <summary>
      /// Provide the -path parameter's string and a Player Profile name to get the path of its Waypoint file.
      /// </summary>
      /// <param name="directory">The path provided by the -path executable parameter.</param>
      /// <param name="profile">The name of the provided Player Profile.</param>
      /// <returns>The Path to the Player Profile's Waypoint file as a string.</returns>
      public static string Waypoint(string directory, string profile)
      {
        return Combine(Profiles(directory), profile, profile);
      }
    }

    public class MCC
    {
      // TODO: Move Steam variables to another class
      public const string Halo1dll = "halo1.dll";
      public const string SteamExe = "steam.exe";
      public const string SteamMccH1 = "steamapps\\common\\Halo The Master Chief Collection\\Halo1";

      /// Determine the path for 32-bit Program Files,
      /// then Steam's default path
      /// and the LibraryFolders.vdf path
      public static readonly string ProgFiles = GetFolderPath(ProgramFilesX86)
                                                != ""
                                                ? GetFolderPath(ProgramFilesX86)
                                                : GetFolderPath(ProgramFiles);
      public static readonly string SteamDefault = Combine(ProgFiles, "Steam");
      
      public static string Steam = SteamDefault; /// Change via SetSteam(steamexepath)
      public static string SteamLibList = Combine(Steam, "steamapps", "libraryfolders.vdf");
      public static string SteamLibrary = Steam; /// Change directly or by assigning an element from Libraries.LibList[]
      public static string Halo1Path = Combine(SteamLibrary, SteamMccH1, Halo1dll); ///

      public static void SetSteam(string steamexepath)
      {
        Steam = GetDirectoryName(steamexepath);
        SteamLibList = Combine(Steam, "steamapps", "libraryfolders.vdf");
        SteamLibrary = Steam;
      }
      /// 1. DONE  Search for "\\Steam\\steamapps\\libraryfolder.vdf".
      /// 2. DONE  Parse the contents for one or more Steam Libary path.
      /// 3. DONE  Walk each library, searching for "\\{library}\\steamapps\\common\\Halo The Master Chief Collection\\halo1\\halo1.dll"
      /// 4. DONE  (OPTIONAL) verify it by checking the file size. If it's above 20MiB, it passes the verification check.
      /// 5. Now, do all of this for custom paths.
    }
  }
}