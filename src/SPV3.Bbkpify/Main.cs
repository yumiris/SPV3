/**
 * Copyright (C) 2018-2019 Emilian Roman
 * 
 * This file is part of SPV3.Bbkpify.
 * 
 * SPV3.Bbkpify is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Bbkpify is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Bbkpify.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using YuMi.Output;

namespace SPV3.Bbkpify
{
  /// <summary>
  ///     Class with core properties or methods.
  /// </summary>
  public static class Main
  {
    /// <summary>
    ///     File extension to use for the backup file.
    /// </summary>
    public const string Extension = "bbkp";

    /// <summary>
    ///     Available bitmap search patterns.
    /// </summary>
    public static readonly List<string> Patterns = new List<string>
    {
      "nrml",
      "multi",
      "diff"
    };

    /// <summary>
    /// Backs up all given bitmaps and replaces them with the given placeholder.
    /// </summary>
    /// <param name="bitmapPaths">Array of bitmaps to back up and replace.</param>
    /// <param name="placeholderPath">Path to the placeholder file.</param>
    public static async Task ApplyPlaceholderAsync(string[] bitmapPaths, string placeholderPath)
    {
      var tasks = new List<Task>();

      for (var i = 0; i < bitmapPaths.Length; i++)
      {
        var file     = bitmapPaths[i];
        var bbkpFile = $"{file}.{Extension}";

        // looks like: [1/10]
        var progress = Ascii.Progress(i, bitmapPaths.Length);

        // check if the current file has been handled in a previous execution
        if (!file.Contains(Extension) && !File.Exists(bbkpFile))
        {
          tasks.Add(Task.Run(() =>
          {
            Line.Write($"{progress}\t| HANDLING {file}", ConsoleColor.Green);

            // backup by renaming, and replace with the placeholder
            File.Move(file, bbkpFile);
            File.Copy(placeholderPath, file);
          }));
        }
        else
        {
          Line.Write($"{progress}\t| SKIPPING {file}", ConsoleColor.Yellow);
        }
      }

      await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Restores the backed up bitmaps and discards the placeholders.
    /// </summary>
    /// <param name="bitmapBbkpPaths">
    /// Bitmaps to restore. They must end in ".bbkp"!
    /// </param>
    /// <exception cref="InvalidEnumArgumentException">
    /// One of the files in the array is not a ".bbkp" file.
    /// </exception>
    public static void ResetBitmapFiles(string[] bitmapBbkpPaths)
    {
      Console.ForegroundColor = ConsoleColor.Green;
      for (var i = 0; i < bitmapBbkpPaths.Length; i++)
      {
        var currentFile = bitmapBbkpPaths[i];

        if (!currentFile.Contains($".{Extension}"))
        {
          throw new InvalidEnumArgumentException($"Bitmap '{currentFile}' is not a backup file.");
        }

        var placeholder = currentFile.Substring(0, currentFile.Length - Extension.Length - 1);

        var progress = Ascii.Progress(i, bitmapBbkpPaths.Length);
        Line.Write($"{progress}\t| RESTORING {placeholder}", ConsoleColor.Green);

        File.Delete(placeholder);
        File.Move(currentFile, placeholder);
      }
    }
  }
}