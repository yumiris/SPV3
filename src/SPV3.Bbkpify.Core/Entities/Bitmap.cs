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

namespace SPV3.Bbkpify.Core.Entities
{
  /// <summary>
  ///   Type of Bitmap that's handled by Sapien.
  /// </summary>
  public enum BitmapType
  {
    Nrml,
    Diff,
    Multi
  }

  /// <summary>
  ///   Entity representing a bitmap file.
  /// </summary>
  public class Bitmap
  {
    /// <summary>
    ///   Extension for the file on the filesystem.
    /// </summary>
    public const string Extension = "bitmap";

    /// <summary>
    ///   <see cref="IsPlaceholder" />
    /// </summary>
    private bool _isPlaceholder;

    /// <summary>
    ///   <see cref="Path" />
    /// </summary>
    private string _path;

    /// <summary>
    ///   <see cref="Size" />
    /// </summary>
    private int _size;

    /// <summary>
    ///   <see cref="BitmapType" />
    /// </summary>
    private BitmapType _type;

    /// <summary>
    ///   Path of the bitmap on the filesystem.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   Path length exceeds 255 characters.
    /// </exception>
    public string Path
    {
      get => _path;
      set
      {
        if (value.Length > 255)
          throw new ArgumentOutOfRangeException(nameof(value), "Path length exceeds 255 characters.");

        _path = value;
      }
    }

    /// <summary>
    ///   Byte length of the bitmap.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   Size length exceeds 16MiB.
    /// </exception>
    public int Size
    {
      get => _size;
      set
      {
        if (value > (2 ^ 24))
          throw new ArgumentOutOfRangeException(nameof(value), "Size length exceeds 16MiB.");

        _size = value;
      }
    }

    /// <summary>
    ///   The bitmap file on the filesystem is a placeholder.
    /// </summary>
    public bool IsPlaceholder
    {
      get => _isPlaceholder;
      set => _isPlaceholder = value;
    }

    /// <summary>
    ///   Bitmap type.
    /// </summary>
    public BitmapType Type
    {
      get => _type;
      set => _type = value;
    }
  }
}