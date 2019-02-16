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
  ///   Type representing a session configuration.
  /// </summary>
  public class Configuration
  {
    /// <summary>
    ///   <see cref="Sapien" />
    /// </summary>
    private Sapien sapien;

    /// <summary>
    ///   Sapien executable.
    /// </summary>
    public Sapien Sapien
    {
      get => sapien;
      set => sapien = value;
    }
  }
}