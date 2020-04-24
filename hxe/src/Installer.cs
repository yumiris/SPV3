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
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using HXE.Properties;
using static System.IO.File;
using static System.IO.Path;
using static HXE.Console;

namespace HXE
{
  /// <summary>
  ///   Installs packages to the filesystem.
  /// </summary>
  public static class Installer
  {
    /// <summary>
    ///   Installs packages from the source directory to the target directory on the filesystem.
    /// </summary>
    /// <param name="source">
    ///   Absolute path to directory containing the installation packages and manifest.
    /// </param>
    /// <param name="target">
    ///   Target directory to install the data from the packages to.
    /// </param>
    /// <param name="progress">
    ///   Optional IProgress object for calling GUI clients.
    /// </param>
    public static void Install(string source, string target, IProgress<Status> progress = null)
    {
      /**
       * Normalisation of the paths will preserve our sanity later on! ;-)
       */

      source = GetFullPath(source);
      target = GetFullPath(target);

      Info("Normalised inbound source and target paths");

      Debug("Source - " + source);
      Debug("Target - " + target);

      if (!Directory.Exists(source))
        throw new DirectoryNotFoundException("Source directory does not exist");

      Info("Source directory exists");

      if (!Directory.Exists(target))
        Directory.CreateDirectory(target);

      Info("Gracefully created target directory");

      var manifest = (Manifest) Combine(source, Paths.Manifest);

      if (!manifest.Exists())
        throw new FileNotFoundException("Manifest file does not exist in the source directory.");

      Info("Manifest binary exists");

      manifest.Load();

      Info("Loaded manifest binary - verifying manifest packages");

      /**
       * Installation is the reversal of the COMPILER routine: we get the data back from the DEFLATE packages, through
       * the use of the generated manifest, and inflate it to the provided target directory on the filesystem.
       */

      foreach (var package in manifest.Packages)
      {
        /**
         * Given that the package filename on the filesystem is expected to match the package's name in the manifest, we
         * infer the package's path by combining the source with the aforementioned name.
         */

        var archive = Combine(source, package.Name);

        if (!Exists(archive))
          throw new FileNotFoundException("Package does not exist in the source directory - " + package.Name);

        Info("Package exists - " + package.Name);
      }

      var c = 1;                       /* current package */
      var t = manifest.Packages.Count; /* total progress  */

      foreach (var package in manifest.Packages)
      {
        progress?.Report(new Status
        {
          Current     = c - 1,
          Total       = t,
          Description = $"Installing: {package.Name} - {package.Entry.Name}"
        });

        var archive   = Combine(source,    package.Name);
        var directory = Combine(target,    package.Entry.Path);
        var file      = Combine(directory, package.Entry.Name);

        if (Exists(file))
        {
          Delete(file);
          Info("Deleted existing file - " + file);
        }

        Directory.CreateDirectory(directory);

        Info("Gracefully created directory - " + package.Entry.Path);

        var task = new Task(() => { ZipFile.ExtractToDirectory(archive, directory); });

        /**
         * While the task is running, we inform the user that is indeed running by updating the console. Aren't we nice
         * people?
         */

        task.Start();

        Wait($"Started package inflation - [{(c * 200 + t) / (t * 2):D3}%] - {package.Name} - {package.Entry.Name} ");

        while (!task.IsCompleted)
        {
          System.Console.Write(Resources.Progress);
          Thread.Sleep(1000);
        }

        if (Exists(file))
          Info("Entry size - " + new FileInfo(file).Length);
        else
          Error("Could not get entry size - " + package.Entry.Name);

        Info("Successfully finished package inflation");

        c++;
      }

      manifest.CopyTo(target);

      Info("Copied manifest to the target directory - " + target);
      Done("Installation routine has been successfully completed");
    }
  }
}