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
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using HXE.HCE;
using static System.Console;
using static System.Environment;
using static System.IO.File;
using static HXE.Console;
using static HXE.Exit;
using static HXE.Properties.Resources;

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
    [STAThread]
    public static void Main(string[] args)
    {
      DisplayBanner();     /* impress our users */
      InvokeProgram(args); /* burn baby burn */
    }

    /// <summary>
    ///   Console API to the HXE kernel, installer and compiler.
    /// </summary>
    /// <param name="args">
    ///   --config            Opens configuration GUI
    ///   --load              Initiates HCE/SPV3
    ///   --install=VALUE     Installs HCE/SPV3   to destination
    ///   --compile=VALUE     Compiles HCE/SPV3   to destination
    ///   --console           Loads HCE           with console mode
    ///   --devmode           Loads HCE           with developer mode
    ///   --screenshot        Loads HCE           with screenshot ability
    ///   --window            Loads HCE           in window mode
    ///   --nogamma           Loads HCE           without gamma overriding
    ///   --adapter=VALUE     Loads HCE           on monitor X
    ///   --path=VALUE        Loads HCE           with custom profile path
    ///   --vidmode=VALUE     Loads HCE           with video mode
    /// </param>
    private static void InvokeProgram(string[] args)
    {
      Directory.CreateDirectory(Paths.Directory);

      var help       = false;        /* Displays commands list              */
      var config     = false;        /* Opens configuration GUI             */
      var positions  = false;        /* Opens positions GUI                 */
      var install    = string.Empty; /* Installs HCE/SPV3 to destination    */
      var compile    = string.Empty; /* Compiles HCE/SPV3 to destination    */
      var update     = string.Empty; /* Updates directory using manifest    */
      var registry   = string.Empty; /* Write to Windows Registry           */
      var console    = false;        /* Loads HCE with console mode         */
      var devmode    = false;        /* Loads HCE with developer mode       */
      var screenshot = false;        /* Loads HCE with screenshot ability   */
      var window     = false;        /* Loads HCE in window mode            */
      var nogamma    = false;        /* Loads HCE without gamma overriding  */
      var adapter    = string.Empty; /* Loads HCE on monitor X              */
      var path       = string.Empty; /* Loads HCE with custom profile path  */
      var exec       = string.Empty; /* Loads HCE with custom init file     */
      var vidmode    = string.Empty; /* Loads HCE with custom res. and Hz   */

      var options = new OptionSet()
        .Add("help",       "Displays commands list",                                  s => help = s       != null) /* hxe command   */
        .Add("config",     "Opens configuration GUI",                                 s => config = s     != null) /* hxe command   */
        .Add("positions",  "Opens positions GUI",                                     s => positions = s  != null) /* hxe command   */
        .Add("install=",   "Installs HCE/SPV3 to destination",                        s => install = s)            /* hxe parameter */
        .Add("compile=",   "Compiles HCE/SPV3 to destination",                        s => compile = s)            /* hxe parameter */
        .Add("update=",    "Updates directory using manifest",                        s => update = s)             /* hxe parameter */
        .Add("registry=",  "Create Registry keys for Retail, Custom, Trial, or HEK",  s => registry = s)           /* hxe parameter */
        .Add("console",    "Loads HCE with console mode",                             s => console = s    != null) /* hce parameter */
        .Add("devmode",    "Loads HCE with developer mode",                           s => devmode = s    != null) /* hce parameter */
        .Add("screenshot", "Loads HCE with screenshot ability",                       s => screenshot = s != null) /* hce parameter */
        .Add("window",     "Loads HCE in window mode",                                s => window = s     != null) /* hce parameter */
        .Add("nogamma",    "Loads HCE without gamma overriding",                      s => nogamma = s    != null) /* hce parameter */
        .Add("adapter=",   "Loads HCE on monitor X",                                  s => adapter = s)            /* hce parameter */
        .Add("path=",      "Loads HCE with custom profile path",                      s => path = s)               /* hce parameter */
        .Add("exec=",      "Loads HCE with custom init file",                         s => exec = s)               /* hce parameter */
        .Add("vidmode=",   "Loads HCE with custom res. and Hz",                       s => vidmode = s);           /* hce parameter */

      var input = options.Parse(args);

      foreach (var i in input)
        Info("Discovered CLI command: " + i);

      var hce = new Executable();

      if (help)
      {
        options.WriteOptionDescriptions(Out);
        Exit(0);
      }

      if (config)
      {
        new Application().Run(new Settings());
        Exit(0);
      }

      if (positions)
      {
        new Application().Run(new Positions());
        Exit(0);
      }

      if (!string.IsNullOrWhiteSpace(install))
        Run(() => { Installer.Install(CurrentDirectory, Path.GetFullPath(install)); });

      if (!string.IsNullOrWhiteSpace(compile))
        Run(() => { Compiler.Compile(CurrentDirectory, Path.GetFullPath(compile)); });

      if (!string.IsNullOrWhiteSpace(update))
        Run(() =>
        {
          var updateModule = new Update();
          updateModule.Import(update);
          updateModule.Commit();
        });

      if (!string.IsNullOrWhiteSpace(registry))
      {

      }



      /**
       * Implicit verification for legal HCE installations.
       */

      try
      {
        hce = Executable.Detect();
      }
      catch (Exception e)
      {
        Error(e.Message + " -- Legal copy of HCE needs to be installed for loading!");
      }

      if (console)
        hce.Debug.Console = true;

      if (devmode)
        hce.Debug.Developer = true;

      if (screenshot)
        hce.Debug.Screenshot = true;

      if (window)
        hce.Video.Window = true;

      if (nogamma)
        hce.Video.NoGamma = true;

      if (!string.IsNullOrWhiteSpace(adapter))
        hce.Video.Adapter = byte.Parse(adapter);

      if (!string.IsNullOrWhiteSpace(path))
        hce.Profile.Path = path;

      if (!string.IsNullOrWhiteSpace(exec))
        hce.Debug.Initiation = exec;

      if (!string.IsNullOrWhiteSpace(vidmode))
      {
        var a = vidmode.Split(',');

        if (a.Length < 2) return;

        hce.Video.Mode   = true;
        hce.Video.Width  = ushort.Parse(a[0]);
        hce.Video.Height = ushort.Parse(a[1]);

        if (a.Length > 2) /* optional refresh rate */
          hce.Video.Refresh = ushort.Parse(a[2]);
      }

      /**
       * Implicitly invoke the HXE kernel with the HCE loading procedure.
       */

      Run(() => { Kernel.Invoke(hce); });

      /**
       * This method is used for running code asynchronously and catching exceptions at the highest level.
       */

      void Run(Action action)
      {
        try
        {
          Task.Run(action).GetAwaiter().GetResult();
          WithCode(Code.Success);
        }
        catch (Exception e)
        {
          Error(e.Message);
          System.Console.Error.WriteLine("\n\n" + e.StackTrace);
          WriteAllText(Paths.Exception, e.ToString());
          WithCode(Code.Exception);
        }
      }
    }

    /// <summary>
    ///   Renders a dynamic banner which is both pleasing to the eye and informative.
    /// </summary>
    private static void DisplayBanner()
    {
      var bn = Assembly.GetEntryAssembly()?.GetName().Version.Major.ToString("D4");

      var bannerLineDecorations = new string('-', BannerBuildSource.Length + 1);

      ForegroundColor = ConsoleColor.Green; /* the colour of the one */
      WriteLine(Banner);                    /* ascii art and usage */
      WriteLine(BannerBuildNumber, bn);     /* reference build */
      WriteLine(bannerLineDecorations);     /* separator */
      WriteLine(BannerBuildSource, bn);     /* reference link */
      WriteLine(bannerLineDecorations);     /* separator */
      ForegroundColor = ConsoleColor.White; /* end banner */
    }
  }
}
