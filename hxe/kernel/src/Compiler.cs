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
using static System.Guid;
using static System.IO.Compression.CompressionLevel;
using static System.IO.Compression.ZipArchiveMode;
using static System.IO.Compression.ZipFile;
using static System.IO.Path;
using static HXE.Console;
using static HXE.Manifest;
using static HXE.Manifest.Package;

namespace HXE
{
  /// <summary>
  ///   Conducts core and data compilation to manifest and compressed packages.
  /// </summary>
  public static class Compiler
  {
    /// <summary>
    ///   Compiles the data in the source directory to packages and a manifest in the target directory.
    /// </summary>
    /// <param name="source">
    ///   Source directory, expected to contain HCE data such as the executables and maps.
    /// </param>
    /// <param name="target">
    ///   Target directory, which is expected to be distributed to the end-user.
    /// </param>
    /// <param name="progress">
    ///   Optional IProgress object for calling GUI clients.
    /// </param>
    public static void Compile(string source, string target, IProgress<Status> progress = null)
    {
      /**
       * Normalisation of the paths will preserve our sanity later on! ^_^
       */

      source = GetFullPath(source); /* normalise source path */
      target = GetFullPath(target); /* normalise target path */

      Info("Normalised inbound source and target paths");

      Debug("Source - " + source);
      Debug("Target - " + target);

      if (!Directory.Exists(source))
        throw new DirectoryNotFoundException("Source directory does not exist.");

      Info("Source directory exists");

      if (!Directory.Exists(target))
        Directory.CreateDirectory(target);

      Info("Gracefully created target directory");

      /**
       * We declare the manifest file to be in the target directory, because the target directory is expected to be the
       * distributed with the installer.
       * 
       * We recursively retrieve a list of files in the source directory, for the purpose of creating packages and
       * manifest entries for each of them.
       * 
       * Given that the manifest is represented by 0x00, the subsequent packages should be represented by a >=1 ID. 
       */

      var manifest    = (Manifest) Combine(target, Paths.Manifest);
      var files       = new DirectoryInfo(source).GetFiles("*", SearchOption.AllDirectories);
      var compression = Optimal;

      /**
       * Yare yare daze, watashi wa aserimasu.
       */

      if (System.IO.File.Exists(Combine(source, "speedwagon")))
      {
        compression = NoCompression;
        Info("No compression will be applied!");
      }

      Info("Retrieved list of files - creating packages");

      /**
       * For any file found in the source, we create a DEFLATE package for it . We also declare an entry for it in the
       * manifest. This permits permits both decompression and verification.
       * 
       * The manifest information seeks to be compatible and portable, by recording only relative paths to the source
       * directory.
       *
       * Each package represents a file in the source directory; hence, we must declare an entry for the respective
       * directory's path relative to the source.
       *
       * This effectively permits two scenarios:
       *
       * -   the INSTALLER can lay out the directories when installing the packages, by combining the particular
       *     target path with the relative path declared for the package.
       * -   the LOADER can infer the absolute path of a file and then match its size against the one declared in the
       *     manifest, as part of its asset verification routine.
       */

      foreach (var file in files)
      {
        var packageName = NewGuid() + ".bin";
        var packagePath = Combine(target, packageName);
        var fileName    = file.Name;

        if (System.IO.File.Exists(packagePath))
        {
          System.IO.File.Delete(packagePath);

          Info("Deleted existing target package - " + packageName);
        }

        /**
         * We record the package's name on the filesystem to the manifest. This permits the INSTALLER to seek the
         * package within the source directory, and then extract its data to the target directory.
         *
         * The packages are conventionally identified by the hexadecimal equivalent of the inbound integer, with ".bin"
         * as the prefix, just like the manifest file.
         */

        manifest.Packages.Add(new Package
        {
          Name = packageName,
          Entry = new PackageEntry
          {
            Name = fileName,
            Size = file.Length,

            /**
             * We MUST declare the RELATIVE path as the value, by inferring the relative path from the absolute path
             * which belongs to the source directory.
             *
             * X:\Development\HCE                  - source absolute path
             * X:\Development\HCE\content\Gallery  - current directory path
             *                   |---------------| - relative path to source
             * 
             * With the above diagram in mind, we can essentially remove the source path portion (and trailing slash)
             * from the full absolute path, and thus declare the relative path as the package's path.
             *
             * NOTE: The aforementioned procedure is not carried out if this iteration's directory is the source
             * directory itself.
             */

            Path = file.DirectoryName != null && file.DirectoryName.Equals(source)
              ? string.Empty
              : file.DirectoryName?.Substring(source.Length + 1)
          }
        });

        Info("Successfully finished package inference - " + packageName + " - " + fileName);
      }

      var c = 1;                       /* current package */
      var t = manifest.Packages.Count; /* total progress  */

      foreach (var package in manifest.Packages)
      {
        progress?.Report(new Status
        {
          Current     = c - 1,
          Total       = t,
          Description = $"Compiling: {package.Name} - {package.Entry.Name}"
        });

        var packagePath = Combine(target, package.Name);

        if (System.IO.File.Exists(packagePath))
        {
          System.IO.File.Delete(packagePath);
          Info("Deleted existing target package - " + package.Name);
        }

        using (var archive = Open(packagePath, Create))
        {
          var task = new Task(() =>
          {
            var entryPath = Combine(source, package.Entry.Path, package.Entry.Name);
            archive.CreateEntryFromFile(entryPath, package.Entry.Name, compression);
          });

          /**
           * While the task is running, we inform the user that is indeed running by updating the console. Aren't we
           * nice people?
           */

          task.Start();

          Wait($"Started package deflation - [{(c * 200 + t) / (t * 2):D3}%] - {package.Name} - {package.Entry.Name} ");

          while (!task.IsCompleted)
          {
            System.Console.Write(Resources.Progress);
            Thread.Sleep(1000);
          }

          package.Size = new FileInfo(packagePath).Length;

          Info("Package size - " + package.Size);
        }

        Info("Successfully finished package deflation");

        c++;
      }

      if (!manifest.Exists())
        manifest.Delete();

      manifest.Save();

      Info("Serialised manifest to the filesystem - " + manifest.Path);

      /**
       * For subsequent installation convenience, we will make a copy of the current CLI to the target directory.
       */

      var cli = (File) Combine(Environment.CurrentDirectory, Paths.Executable);

      if (cli.Exists())
        cli.CopyTo(target);

      Info("Copied current HXE assembly to the target directory");
      Done("Compilation routine has been successfully completed");
    }
  }
}