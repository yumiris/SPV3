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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using HXE.Exceptions;
using HXE.Properties;
using static System.Environment;
using static System.IO.Compression.ZipFile;
using static SevenZip.Compression.LZMA.Decoder;
using static System.IO.File;
using static System.IO.Path;
using static System.Net.Cache.HttpRequestCacheLevel;
using static HXE.Console;

namespace HXE
{
  /// <summary>
  ///   Module for updating assets from a specified manifest file.
  /// </summary>
  public class Update
  {
    /// <summary>
    ///   Assets available to download for updating.
    /// </summary>
    public List<Asset> Assets { get; set; } = new List<Asset>();

    /// <summary>
    ///   Imports assets from the specified file location.
    /// </summary>
    /// <param name="uri">
    ///   Location of the file. Can be either a file on the filesystem, or a HTTP(S) URI.
    /// </param>
    public void Import(string uri)
    {
      string data;

      /**
       * HXE is a nimble little beast that likes to check both the file system and the webs for the inbound manifest.
       *
       * Its first step is to check the file system for a file matching the inbound URI. If it exists, then we may rest
       * assured that it's a file the end-user has specified. Otherwise, it's probably a web request. If it's not, then,
       * err... we're in a bit of a pickle.
       */

      if (Exists(uri)) /* retrieve data from the file system */
      {
        Info("Inferred filesystem manifest - " + uri);

        using (var stream = new StreamReader(System.IO.File.OpenRead(uri)))
        {
          data = stream.ReadToEnd();
        }
      }
      else /* retrieve data from web resource (http request) */
      {
        Info("Inferred web request manifest - " + uri);

        var req = WebRequest.Create(uri);
        req.CachePolicy = new HttpRequestCachePolicy(NoCacheNoStore);

        using (var wr = (HttpWebResponse) req.GetResponse())
        using (var rs = wr.GetResponseStream())
        using (var sr = new StreamReader(rs ?? throw new NullReferenceException("No response for manifest.")))
        {
          data = sr.ReadToEnd();
        }
      }

      Info("Downloaded XML data from the manifest");
      Debug(data);

      /**
       * It's expected that the contents of the inbound resource are an XML representation of the Update object. Any
       * wise person should have created the respective resource using this object's Export method.
       */

      using (var reader = new StringReader(data))
      {
        var update = (Update) new XmlSerializer(typeof(Update)).Deserialize(reader);
        Assets = update.Assets;
      }

      foreach (var asset in Assets) Info("Inferred asset - " + asset.Name);

      Done("Update import routine has been successfully completed");
    }

    /// <summary>
    ///   Checks if update exists based on the current environment state.
    /// </summary>
    /// <returns>
    ///   True on updating being possible, otherwise false.
    /// </returns>
    public bool Available()
    {
      foreach (var asset in Assets)
      {
        var path = Combine(CurrentDirectory, asset.Path, asset.Name);

        if (!Exists(path))
          return true;

        var length = new FileInfo(path).Length;

        if (length != asset.Size)
          return true;
      }

      return false;
    }

    /// <summary>
    ///   Serialises object state to an inbound file on the filesystem.
    /// </summary>
    /// <param name="uri">
    ///   Path to serialise object state to.
    /// </param>
    public void Export(string uri)
    {
      /**
       * A nimble little XML dumper for this object's state. Nothing more, nothing less!
       */

      using (var writer = new StreamWriter(uri))
      {
        new XmlSerializer(typeof(Update)).Serialize(writer, this);
      }

      Done("Update export routine has been successfully completed");
    }

    /// <summary>
    ///   Conducts the update mechanism on each asset.
    /// </summary>
    /// <param name="progress">
    ///   Optional IProgress object for calling GUI clients.
    /// </param>
    public void Commit(IProgress<Status> progress = null)
    {
      Info("Started asset update routine - " + Assets.Count + " assets");

      foreach (var asset in Assets)
      {
        /**
         * If the asset matches a file which exists at the target destination on the file system, then re-installing it
         * would be pointless.
         * 
         * Using byte length for comparing file sizes is particularly naïve, but it does the job for now. Should use a
         * hash later on!
         */

        var target = Combine(CurrentDirectory, asset.Path, asset.Name);

        if (Exists(target))
          if (new FileInfo(target).Length == asset.Size)
            continue;

        asset.Request(progress); /* grab our package */
        asset.Install(progress); /* inflate its data */
        asset.CleanUp();         /* clean up package */
      }

      Done("Finished asset update routine - " + Assets.Count + " assets");
    }

    public class Asset
    {
      private readonly string File = Combine(CurrentDirectory, Guid.NewGuid().ToString());

      public string URL  { get; set; } = string.Empty; /* http path for downloading the binary to the file system  */
      public string Name { get; set; } = string.Empty; /* expected name for the binary once it has been downloaded */
      public string Path { get; set; } = string.Empty; /* path relative to the working folder for storing the file */
      public long   Size { get; set; }                 /* byte size of the binary on for length-based verification */

      /// <summary>
      ///   Downloads the asset's package to the filesystem for subsequent installation.
      /// </summary>
      public void Request(IProgress<Status> progress = null)
      {
        /**
         * Let's just hope that this isn't invoked from a read-only directory.
         */

        using (var client = new WebClient())
        {
          client.DownloadProgressChanged += (s, e) =>
          {
            progress?.Report(new Status
            {
              Current     = e.BytesReceived,
              Total       = e.TotalBytesToReceive,
              Description = $"Requesting: {Name} ({(decimal) e.BytesReceived / e.TotalBytesToReceive:P})"
            });
          };

          var task = new Task(() => { client.DownloadFile(URL, File); });

          task.Start();

          Wait($"Started asset download - {Name} - {URL} ");

          while (!task.IsCompleted)
          {
            System.Console.Write(Resources.Progress);
            Thread.Sleep(1000);
          }

          Done("Asset request has been successfully completed");
        }
      }

      /// <summary>
      ///   Inflates the asset's package.
      /// </summary>
      /// <exception cref="AssetException">
      ///   Package does not exist. It should be downloaded!
      /// </exception>
      public void Install(IProgress<Status> progress = null)
      {
        if (!Exists(File))
          throw new AssetException("Package not found for asset - " + Name);

        Info("Asset package found on the filesystem");

        var directory = Combine(CurrentDirectory, Path); /* destination directory */
        var target    = Combine(directory,        Name); /* real file path on fs  */
        var backup    = target + "-" + Guid.NewGuid();

        Directory.CreateDirectory(directory);

        Info("Asset deemed suitable to update/install");

        /**
         * This is almost an installer on steroids!
         */
        if (Name.EndsWith(".7z"))
        {
          try
          {

          }
          catch(Exception)
          {

          }
        }
        else try
        {
          if (Exists(target))
            Move(target, backup);

          var task = new Task(() => { ExtractToDirectory(File, directory); });

          task.Start();

          Wait($"Started package inflation - {Name} - {target} ");

          while (!task.IsCompleted)
          {
            if (Exists(target))
            {
              var c = new FileInfo(target).Length;
              var t = Size;

              progress?.Report(new Status
              {
                Current     = c,
                Total       = t,
                Description = $"Installing: {Name} ({(decimal) c / t:P})"
              });
            }

            System.Console.Write(Resources.Progress);
            Thread.Sleep(1000);
          }

          if (Exists(backup))
            Delete(backup);

          Done("Asset install has been successfully completed");
        }
        catch (Exception)
        {
          if (!Exists(backup)) throw;

          if (Exists(target))
            Delete(target);

          Move(backup, target);

          throw;
        }
      }

      /// <summary>
      ///   Removes the download package if it exists on the filesystem.
      /// </summary>
      public void CleanUp()
      {
        if (Exists(File))
          Delete(File);

        Done("Asset cleanup has been successfully completed");
      }
    }
  }
}