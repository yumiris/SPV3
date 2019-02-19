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
  ///   Type representing the latest recorded session.
  /// </summary>
  public class History
  {
    /// <summary>
    ///   <see cref="Session" />
    /// </summary>
    private Session _session;

    /// <summary>
    ///   <see cref="Time" />
    /// </summary>
    private DateTime _time;

    /// <summary>
    ///   Historical session object.
    /// </summary>
    public Session Session
    {
      get => _session;
      set => _session = value;
    }

    /// <summary>
    ///   Timestamp of the last session.
    /// </summary>
    public DateTime Time
    {
      get => _time;
      set => _time = value;
    }
  }
}