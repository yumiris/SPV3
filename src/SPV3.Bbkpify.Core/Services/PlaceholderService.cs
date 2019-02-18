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
using SPV3.Bbkpify.Core.Entities;

namespace SPV3.Bbkpify.Core.Services
{
  /// <summary>
  ///   Provides methods for conducting the replacing of a bitmap with a placeholder.
  /// </summary>
  public class PlaceholderService
  {
    /// <summary>
    ///   Handles an inbound directory's list of bitmaps by replacing them with the inbound placeholder.
    /// </summary>
    /// <param name="directory">
    ///   Directory with bitmaps on the filesystem.
    /// </param>
    /// <param name="placeholder">
    ///   Bitmap placeholder to apply.
    /// </param>
    public void Handle(Directory directory, Bitmap placeholder)
    {
      throw new NotImplementedException();
    }
  }
}