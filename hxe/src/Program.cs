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
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using static System.Console;
using static System.Environment;
using static System.IO.File;
using static HXE.Console;
using static HXE.Exit;

namespace HXE
{
  /// <summary>
  ///   HXE Program.
  /// </summary>
  internal static class Program
  {
    /// <summary>
    ///   HXE entry.
    /// </summary>
    /// <param name="args">
    ///   Arguments for HXE.
    /// </param>
    public static void Main(string[] args)
    {
      var bn = Assembly.GetEntryAssembly().GetName().Version.Major.ToString("D4");

      ForegroundColor = ConsoleColor.Green;
      WriteLine(@" _    ___   ________ ");
      WriteLine(@"| |  | \ \ / /  ____|");
      WriteLine(@"| |__| |\ V /| |__   ");
      WriteLine(@"|  __  | > < |  __|  ");
      WriteLine(@"| |  | |/ . \| |____ ");
      WriteLine(@"|_|  |_/_/ \_\______| :: Halo XE");
      WriteLine(@"================================");
      WriteLine(@"A HCE wrapper and SPV3.2 loader.");
      WriteLine(@"--------------------------------");
      WriteLine(@"src: https://cgit.n2.network/hxe");
      WriteLine(@"bin: https://dist.n2.network/hxe");
      WriteLine(@"--------------------------------");
      WriteLine($"Current binary build number {bn}");
      WriteLine(@"--------------------------------");
      ForegroundColor = ConsoleColor.White;

      Directory.CreateDirectory(Paths.Directories.HXE);

      var hce = Executable.Detect();

      var options = new OptionSet()
        .Add("load", "Initiates HCE/SPV3",
          s => Run(() => { Kernel.Bootstrap(hce); }))
        .Add("install=", "Installs HCE/SPV3 to destination",
          s => Run(() => { Installer.Install(CurrentDirectory, s); }))
        .Add("compile=", "Compiles HCE/SPV3 to destination",
          s => Run(() => { Compiler.Compile(CurrentDirectory, s); }))
        .Add("console", "Loads HCE with console mode",
          s => hce.Debug.Console = true)
        .Add("devmode", "Loads HCE with developer mode",
          s => hce.Debug.Developer = true)
        .Add("screenshot", "Loads HCE with screenshot ability",
          s => hce.Debug.Screenshot = true)
        .Add("window", "Loads HCE in window mode",
          s => hce.Video.Window = true)
        .Add("adapter=", "Loads HCE on monitor X",
          s => hce.Video.Adapter = int.Parse(s))
        .Add("path=", "Loads HCE with custom profile path",
          s => hce.Profile.Path = s)
        .Add("vidmode=", "Loads HCE with video mode",
          s =>
          {
            var a = s.Split(',');

            if (a.Length != 3) return;

            hce.Video.Width   = int.Parse(a[0]);
            hce.Video.Height  = int.Parse(a[1]);
            hce.Video.Refresh = int.Parse(a[2]);
          });

      options.WriteOptionDescriptions(Out);
      var input = options.Parse(args);

      if (!input.Contains("load")    &&
          !input.Contains("install") &&
          !input.Contains("compile"))
        Run(() => { Kernel.Bootstrap(hce); });
    }

    private static void Run(Action action)
    {
      try
      {
        Task.Run(action).GetAwaiter().GetResult();
        WithCode(Code.Success);
      }
      catch (Exception e)
      {
        Error(e.Message);
        System.Console.Error.WriteLine(e.StackTrace);
        WriteAllText(Paths.Files.Exception, e.ToString());
        WithCode(Code.Exception);
      }
    }
  }
}