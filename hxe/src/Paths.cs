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

using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.File;
using static System.IO.Path;

namespace HXE
{
  /// <summary>
  ///   Lists all of the files & directories on the filesystem that HCE/HXE deals with.
  /// </summary>
  public static class Paths
  {
    public const string Executable = "hxe.exe";
    public const string Manifest   = "manifest.bin";

    public static readonly string Directory     = Combine(GetFolderPath(ApplicationData), "HXE");
    public static readonly string Configuration = Combine(Directory,                      "kernel.bin");
    public static readonly string Exception     = Combine(Directory,                      "exception.log");
    public static readonly string Positions     = Combine(CurrentDirectory,               "positions.bin");
    public static readonly string DSOAL         = Combine(CurrentDirectory,               "dsoal-aldrv.dll");
    public static readonly string DSOUND        = Combine(CurrentDirectory,               "dsound.dll");
    public static readonly string ALSoft        = Combine(CurrentDirectory,               "alsoft.ini");
    public static readonly string Version       = Combine(CurrentDirectory,               "version.txt");

    public class HCE
    {
      public const           string Executable = "haloce.exe";
      public static readonly string Directory  = Combine(GetFolderPath(Personal), "My Games", "Halo CE");

      public static readonly string Profiles    = Combine(Directory, "savegames");
      public static readonly string LastProfile = Combine(Directory, "lastprof.txt");
      public static readonly string Chimera     = Combine(Directory, "chimera.bin");
      public static readonly string OpenSauce   = Combine(Directory, "OpenSauce", "OS_Settings.User.xml");

      /*
       * The initiation filename is declared based on the presence of the OpenSauce DLL in the current directory.
       * 
       * -   initc is interpreted by OpenSauce (InitC = Initiation Client file)
       * -   init is primarily used by dedicated HCE servers for startup commands
       *
       * See #20 on github:SPV3
       */
      public static readonly string Initiation
        = Combine(CurrentDirectory, Exists("dinput8.dll") ? "initc.txt" : "init.txt");

      public static string Profile(string profile)
      {
        return Combine(Directory, Profiles, profile, "blam.sav");
      }

      public static string Progress(string profile)
      {
        return Combine(Directory, Profiles, profile, "savegame.bin");
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

      public static string Profile(string directory, string profile)
      {
        return Combine(directory, Profiles(directory), profile, "blam.sav");
      }

      public static string Progress(string directory, string profile)
      {
        return Combine(directory, Profiles(directory), profile, "savegame.bin");
      }
    }
  }
}