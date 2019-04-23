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
using System.Text;
using static System.Environment;
using static System.IO.File;
using static System.IO.Path;

namespace SPV3.CLI
{
  /// <summary>
  ///   Object representing a Placeholder bitmap on the filesystem.
  /// </summary>
  public static class Placeholder
  {
    /// <summary>
    ///   Extension used for backed up bitmaps.
    /// </summary>
    private const string Extension = ".bbkp";

    /// <summary>
    ///   Replaces the filtered files in the target directory with the placeholder file.
    /// </summary>
    /// <param name="placeholder">
    ///   Placeholder bitmap to replace normal bitmaps with.
    /// </param>
    /// <param name="target">
    ///   Target directory containing the files which should be replaced with the placeholder.
    /// </param>
    /// <param name="filter">
    ///   Regex filter the files which should be handled in the target directory.
    /// </param>
    public static void Commit(string placeholder, string target, string filter)
    {
      var files  = Directory.GetFiles(target, filter, SearchOption.AllDirectories);
      var record = new StringBuilder();

      foreach (var file in files)
      {
        Move(file, file + Extension);
        Copy(placeholder, file);
        record.AppendLine(file);
      }

      WriteAllText(Combine(CurrentDirectory, Guid.NewGuid() + ".txt"), record.ToString());
    }

    /// <summary>
    ///   Restores bitmaps based on the provided records file.
    /// </summary>
    /// <param name="records">
    ///   Records text file created by the commit method.
    /// </param>
    public static void Revert(string records)
    {
      using (var reader = new StringReader(ReadAllText(records)))
      {
        string line;
        do
        {
          line = reader.ReadLine();
          if (line == null) continue;

          var target = line;
          var source = target + Extension;

          if (Exists(target))
            Delete(target);

          if (Exists(source))
            Move(source, target);
        } while (line != null);
      }
    }
  }
}