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

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Directory = System.IO.Directory;
using static HXE.Console;
using static HXE.Paths;
using static System.Environment;

namespace HXE.HCE
{
  /// <inheritdoc />
  /// <summary>
  ///   Object representation of a lastprof.txt file.
  /// </summary>
  public class LastProfile : File
  {
    /// <summary>
    ///   Last accessed HCE profile.
    /// </summary>
    public string Profile { get; set; } = "New001";

    /// <summary>
    ///   Interprets the profile name from the lastprof.txt file.
    /// </summary>
    public void Load()
    {
      try
      {
        var data   = ReadAllText();
        var split  = data.Split('\\');
        var offset = split.Length - 2;
        Profile    = split[offset];
      }
      catch (System.IndexOutOfRangeException e)
      {
        var msg = " -- LASTPROFILE.LOAD FAILED" + NewLine
                + " Error:  " + e.ToString()    + NewLine;
        var log = (File) Exception;
        log.AppendAllText(msg);
        Error(e.Message + " -- LASTPROFILE.LOAD FAILED");
        
        Core("Recreating LastProf.txt...");
        {
          Profile firstProfile = new Profile();
          string userPath = System.IO.Path.GetDirectoryName(Path);
          try
          {
            firstProfile = HCE.Profile.List(Custom.Profiles(userPath)).First();
            // TODO : validate profile. Is it corrupted?
            Name = firstProfile.Name;
          }
          catch (System.Exception)
          {
            NewProfile.Generate(userPath, this, firstProfile, Directory.Exists(Custom.Profiles(userPath)));
          }
        }
      } 
    }

    /// <summary>
    ///   Writes the mutated profile value to the file system.
    /// </summary>
    public void Save()
    {
      using (var fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
      using (var ms = new MemoryStream(255))
      using (var bw = new BinaryWriter(ms))
      {
        var path = System.IO.Path.GetDirectoryName(Path);
        byte[] profdir = Encoding.UTF8.GetBytes(Custom.ProfileDirectory(path, Profile));
        byte[] delim = Encoding.UTF8.GetBytes("\\");
        byte[] pad = { 0 };
        byte[] blam = Encoding.UTF8.GetBytes("lam.sav");
        List<byte> list1 = new List<byte>(profdir);
        List<byte> list2 = new List<byte>(delim);
        List<byte> list3 = new List<byte>(pad);
        List<byte> list4 = new List<byte>(blam);

        list1.AddRange(list2);
        list1.AddRange(list3);
        list1.AddRange(list4);
        byte[] stream = list1.ToArray();

        var sb = new StringBuilder("Byte Array {");
        foreach (var b in stream)
        {
          sb.Append(b + " ");
        }
        sb.Append("}");
        Debug(sb.ToString());

        ms.Position = 0;
        bw.Write(stream);

        ms.Position = 0;
        ms.CopyTo(fs);

        Info("lastprof.txt created");
      }
    }

    /// <summary>
    ///   Represents the inbound object as a string.
    /// </summary>
    /// <param name="lastProfile">
    ///   Object to represent as string.
    /// </param>
    /// <returns>
    ///   String representation of the inbound object.
    /// </returns>
    public static implicit operator string(LastProfile lastProfile)
    {
      return lastProfile.Path;
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
    public static explicit operator LastProfile(string name)
    {
      return new LastProfile
      {
        Path = name
      };
    }
  }
}