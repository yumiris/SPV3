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

namespace SPV3.Bbkpify
{
  /// <summary>
  ///     Common validity statuses for a given bitmaps directory.
  /// </summary>
  public enum DirectoryStatus
  {
    /// <summary>
    ///     Bitmaps directory is valid and exists.
    /// </summary>
    IsValid,

    /// <summary>
    ///     Bitmaps directory does not exist.
    /// </summary>
    DoesNotExist
  }

  /// <summary>
  ///     Common validity statuses for a given placeholder file.
  /// </summary>
  public enum PlaceholderStatus
  {
    /// <summary>
    ///     Placeholder file is valid in existence and size.
    /// </summary>
    IsValid,

    /// <summary>
    ///     Placeholder file does not exist at the given path.
    /// </summary>
    DoesNotExist,

    /// <summary>
    ///     Placeholder file is larger than 8MiB.
    /// </summary>
    IsTooLarge
  }

  public enum PatternStatus
  {
    /// <summary>
    ///     Bitmaps search pattern is valid.
    /// </summary>
    IsValid,

    /// <summary>
    ///     Bitmaps search pattern is invalid.
    /// </summary>
    IsInvalid
  }
}