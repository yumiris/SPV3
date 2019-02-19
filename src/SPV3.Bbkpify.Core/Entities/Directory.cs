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

using System.Collections.Generic;

namespace SPV3.Bbkpify.Core.Entities
{
  /// <summary>
  ///   Type representing a directory with bitmaps.
  /// </summary>
  public class Directory
  {
    /// <summary>
    ///   <see cref="Path"/>
    /// </summary>
    private Path _path;

    /// <summary>
    ///   <see cref="Bitmaps" />
    /// </summary>
    private List<Bitmap> _bitmaps;

    /// <summary>
    ///   List of bitmaps in the directory.
    /// </summary>
    public List<Bitmap> Bitmaps
    {
      get => _bitmaps;
      set => _bitmaps = value;
    }

    /// <summary>
    ///   Directory path on the filesystem.
    /// </summary>
    public Path Path
    {
      get => _path;
      set => _path = value;
    }
  }
}