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
using System.Threading.Tasks;
using Mono.Options;
using static System.Console;
using static System.Environment;
using static System.Int32;
using static System.Reflection.Assembly;
using static SPV3.CLI.Console;
using static SPV3.CLI.Exit.Code;

namespace SPV3.CLI
{
  /// <summary>
  ///   SPV3.CLI Program.
  /// </summary>
  internal static class Program
  {
    /// <summary>
    ///   SPV3.CLI entry.
    /// </summary>
    /// <param name="args">
    ///   Arguments for the CLI.
    /// </param>
    public static void Main(string[] args)
    {
      var v = GetEntryAssembly().GetName().Version.Major.ToString("D3");

      ForegroundColor = ConsoleColor.Green;
      WriteLine(@"   _____ ____ _    _______  ________    ____");
      WriteLine(@"  / ___// __ \ |  / /__  / / ____/ /   /  _/");
      WriteLine(@"  \__ \/ /_/ / | / / /_ < / /   / /    / /  ");
      WriteLine(@" ___/ / ____/| |/ /___/ // /___/ /____/ /   ");
      WriteLine(@"/____/_/     |___//____(_)____/_____/___/   ");
      WriteLine(@"============================================");
      WriteLine(@"The SPV3 CLI ~ Automatic loader for HCE/SPV3");
      WriteLine(@"--------------------------------------------");
      WriteLine(@"source  ::  https://cgit.n2.network/spv3.cli");
      WriteLine(@"binary  ::  https://dist.n2.network/spv3.cli");
      WriteLine(@"--------------------------------------------");
      WriteLine($"Executable has been compiled from build: {v}");
      WriteLine(@"--------------------------------------------");
      ForegroundColor = ConsoleColor.White;

      Directory.CreateDirectory(Paths.Directories.SPV3);

      var hce = Executable.Detect();

      var options = new OptionSet()
        .Add("load", "Initiates HCE/SPV3",
          s => Run(() => { Kernel.Bootstrap(hce); }))
        .Add("install=", "Installs SPV3 to destination",
          s => Run(() => { Installer.Install(CurrentDirectory, s); }))
        .Add("compile=", "Compiles SPV3 to destination",
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
          s => hce.Video.Adapter = Parse(s))
        .Add("path=", "Loads HCE with custom profile path",
          s => hce.Profile.Path = s)
        .Add("vidmode=", "Loads HCE with video mode",
          s =>
          {
            var a = s.Split(',');

            if (a.Length != 3) return;

            hce.Video.Width   = Parse(a[0]);
            hce.Video.Height  = Parse(a[1]);
            hce.Video.Refresh = Parse(a[2]);
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
        Exit.WithCode(Success);
      }
      catch (Exception e)
      {
        Error(e.Message);
        System.Console.Error.WriteLine(e.StackTrace);
        System.IO.File.WriteAllText(Paths.Files.Exception, e.ToString());
        Exit.WithCode(Exit.Code.Exception);
      }
    }
  }
}