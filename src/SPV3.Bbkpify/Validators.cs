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

using System.IO;

namespace SPV3.Bbkpify
{
  /// <summary>
  ///     Bitmap placeholder file validation checking.
  /// </summary>
  public static class PlaceholderValidator
  {
    /// <summary>
    ///     Maximum placeholder file size.
    /// </summary>
    private const int SafeFileSize = 0x800000;

    /// <summary>
    ///     Returns the validity status of a given placeholder.
    /// </summary>
    /// <param name="placeholderPath">
    ///     Path of the placeholder file.
    /// </param>
    /// <returns>
    ///     PlaceholderStatus enum value.
    /// </returns>
    public static PlaceholderStatus GetStatus(string placeholderPath)
    {
      if (!File.Exists(placeholderPath))
      {
        return PlaceholderStatus.DoesNotExist;
      }

      if (new FileInfo(placeholderPath).Length > SafeFileSize)
      {
        return PlaceholderStatus.IsTooLarge;
      }

      return PlaceholderStatus.IsValid;
    }
  }

  /// <summary>
  ///     Bitmaps directory validation checking.
  /// </summary>
  public static class DirectoryValidator
  {
    /// <summary>
    ///     Returns the validity status of a given directory.
    /// </summary>
    /// <param name="directoryPath">
    ///     Path to the bitmaps directory.
    /// </param>
    /// <returns>
    ///     DirectoryStatus enum value.
    /// </returns>
    public static DirectoryStatus GetStatus(string directoryPath)
    {
      if (!Directory.Exists(directoryPath))
      {
        return DirectoryStatus.DoesNotExist;
      }

      return DirectoryStatus.IsValid;
    }
  }

  /// <summary>
  ///     Search pattern validation checking.
  /// </summary>
  public static class PatternValidator
  {
    /// <summary>
    ///     Returns the validity status of a given bitmap search pattern.
    /// </summary>
    /// <param name="searchPattern">
    ///     Bitmap file search pattern.
    /// </param>
    /// <returns>
    ///     PatternStatus enum value.
    /// </returns>
    public static PatternStatus GetStatus(string searchPattern)
    {
      if (!Main.Patterns.Contains(searchPattern))
      {
        return PatternStatus.IsInvalid;
      }

      return PatternStatus.IsValid;
    }
  }
}