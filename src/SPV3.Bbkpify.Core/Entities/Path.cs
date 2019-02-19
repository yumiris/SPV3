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
  /// 
  /// </summary>
  public class Path
  {
    /// <summary>
    ///   <see cref="Value" />
    /// </summary>
    private string _value;

    /// <summary>
    ///   Path of the bitmap on the filesystem.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   Path length exceeds 255 characters.
    /// </exception>
    public string Value
    {
      get => _value;
      set
      {
        if (value.Length > 255)
          throw new ArgumentOutOfRangeException(nameof(value), "Path length exceeds 255 characters.");

        _value = value;
      }
    }
  }
}