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

using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Serialization;

namespace SPV3.CLI
{
  /// <summary>
  ///   Manifest created by the compiler and parsed by the installer & loader.
  /// </summary>
  public class Manifest : File
  {
    public List<Package> Packages { get; set; } = new List<Package>();

    /// <summary>
    ///   Serialises current object to persistent data on the filesystem.
    /// </summary>
    public void Save()
    {
      var    mode = CompressionMode.Compress;
      byte[] data;

      using (var writer = new StringWriter())
      {
        var serialiser = new XmlSerializer(typeof(Manifest));
        serialiser.Serialize(writer, this);
        data = Encoding.UTF8.GetBytes(writer.ToString());
      }

      using (var inflatedStream = new MemoryStream(data))
      using (var deflatedStream = new MemoryStream())
      using (var compressStream = new DeflateStream(deflatedStream, mode))
      {
        inflatedStream.CopyTo(compressStream);
        compressStream.Close();
        WriteAllBytes(deflatedStream.ToArray());
      }
    }

    /// <summary>
    ///   Deserialises persisted filesystem data to the current object.
    /// </summary>
    public void Load()
    {
      var    mode = CompressionMode.Decompress;
      string data;

      using (var inflatedStream = new MemoryStream())
      using (var deflatedStream = new MemoryStream(ReadAllBytes()))
      using (var compressStream = new DeflateStream(deflatedStream, mode))
      {
        compressStream.CopyTo(inflatedStream);
        compressStream.Close();
        data = Encoding.UTF8.GetString(inflatedStream.ToArray());
      }

      using (var reader = new StringReader(data))
      {
        var serialiser = new XmlSerializer(typeof(Manifest));
        var serialised = (Manifest) serialiser.Deserialize(reader);
        Packages = serialised.Packages;
      }
    }

    /// <summary>
    ///   Represents the inbound object as a string.
    /// </summary>
    /// <param name="manifest">
    ///   Object to represent as string.
    /// </param>
    /// <returns>
    ///   String representation of the inbound object.
    /// </returns>
    public static implicit operator string(Manifest manifest)
    {
      return manifest.Path;
    }

    /// <summary>
    ///   Represents the inbound string as an object.
    /// </summary>
    /// <param name="name">
    ///   String to represent as object.
    /// </param>
    /// <returns>
    ///   Object representation of the inbound string.
    /// </returns>
    public static explicit operator Manifest(string name)
    {
      return new Manifest
      {
        Path = name
      };
    }

    /// <summary>
    ///   Manifest member representing a package on the filesystem with installable entries.
    /// </summary>
    public class Package
    {
      public string      Name    { get; set; }                      // Package filename on the filesystem
      public string      Path    { get; set; } = string.Empty;      // Path relative to root source/target dir
      public List<Entry> Entries { get; set; } = new List<Entry>(); // Package entries == directory files

      /// <summary>
      ///   Represents the inbound object as a string.
      /// </summary>
      /// <param name="package">
      ///   Object to represent as string.
      /// </param>
      /// <returns>
      ///   String representation of the inbound object.
      /// </returns>
      public static implicit operator string(Package package)
      {
        return package.Path;
      }

      /// <summary>
      ///   Represents the inbound string as an object.
      /// </summary>
      /// <param name="name">
      ///   String to represent as object.
      /// </param>
      /// <returns>
      ///   Object representation of the inbound string.
      /// </returns>
      public static explicit operator Package(string name)
      {
        return new Package
        {
          Name = name
        };
      }

      /// <summary>
      ///   Package entry representing a file on the filesystem or entry in the package.
      /// </summary>
      public class Entry
      {
        public string Name { get; set; } // Filename with extension relative to dir
        public long   Size { get; set; } // Bytes length of the entry on filesystem
      }
    }
  }
}