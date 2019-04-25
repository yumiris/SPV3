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
using System.Xml.Serialization;
using static System.IO.Path;

namespace HXE
{
  /// <summary>
  ///   Object defining domain rules for a file on the filesystem, and exposing common file manipulation & management
  ///   methods.
  /// </summary>
  public class File
  {
    private string _path;

    [XmlIgnore]
    public string Path
    {
      get => _path;
      set
      {
        if (value.Length > 255)
          throw new ArgumentOutOfRangeException(nameof(value), "File path exceeds 255 chars.");

        _path = value;
        Name  = GetFileName(_path);
      }
    }

    public string Name { get; set; }

    public void CreateDirectory()
    {
      var baseDirectory = GetDirectoryName(Path);

      if (!Directory.Exists(baseDirectory))
        Directory.CreateDirectory(baseDirectory ?? throw new ArgumentNullException());
    }

    public bool Exists()
    {
      return System.IO.File.Exists(Path);
    }

    public void Delete()
    {
      System.IO.File.Delete(Path);
    }

    public void CopyTo(string target)
    {
      System.IO.File.Copy(Path, Combine(target, Name));
    }

    public void WriteAllText(string contents)
    {
      CreateDirectory();
      System.IO.File.WriteAllText(Path, contents);
    }

    public void WriteAllBytes(byte[] bytes)
    {
      CreateDirectory();
      System.IO.File.WriteAllBytes(Path, bytes);
    }

    public string ReadAllText()
    {
      return System.IO.File.ReadAllText(Path);
    }

    public byte[] ReadAllBytes()
    {
      return System.IO.File.ReadAllBytes(Path);
    }

    /// <summary>
    ///   Represents the inbound object as a string.
    /// </summary>
    /// <param name="file">
    ///   Object to represent as string.
    /// </param>
    /// <returns>
    ///   String representation of the inbound object.
    /// </returns>
    public static implicit operator string(File file)
    {
      return file.Path;
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
    public static explicit operator File(string name)
    {
      return new File
      {
        Path = name
      };
    }
  }
}