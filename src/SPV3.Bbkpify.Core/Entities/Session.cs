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

namespace SPV3.Bbkpify.Core.Entities
{
  /// <summary>
  ///   Type representing a bitmap replacement session.
  /// </summary>
  public class Session
  {
    /// <summary>
    ///   <see cref="Directory" />
    /// </summary>
    private Directory _directory;

    /// <summary>
    ///   <see cref="Placeholder" />
    /// </summary>
    private Bitmap _placeholder;

    /// <summary>
    ///   <see cref="Type" />
    /// </summary>
    private BitmapType _type;

    /// <summary>
    ///   Target directory with bitmaps to replace with placeholder.
    /// </summary>
    public Directory Directory
    {
      get => _directory;
      set => _directory = value;
    }

    /// <summary>
    ///   Placeholder to apply to the bitmaps in the <see cref="Directory" />.
    /// </summary>
    public Bitmap Placeholder
    {
      get => _placeholder;
      set => _placeholder = value;
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