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
using System.IO;
using SPV3.Bbkpify.Core.Entities;
using SPV3.Bbkpify.Infrastructure.Common;
using Path = System.IO.Path;

namespace SPV3.Bbkpify.Infrastructure.Data
{
  /// <summary>
  ///   Repository which persists a Session object to a binary on the filesystem.
  /// </summary>
  public class SessionRepository : Repository
  {
    /// <summary>
    ///   Default binary used for session persistence.
    /// </summary>
    private const string Binary = "SPV3.Bbkpify.Session.bin";

    /// <summary>
    ///   Path for session object data I/O.
    /// </summary>
    private readonly string _path;

    /// <summary>
    ///   SessionRepository constructor.
    /// </summary>
    /// <param name="path">
    ///   Path for session object data I/O.
    /// </param>
    public SessionRepository(string path = null)
    {
      if (path != null)
      {
        _path = path;
        return;
      }

      var appData  = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      var fullPath = Path.Combine(appData, Binary);

      _path = fullPath;
    }

    /// <summary>
    ///   Saves inbound Session object to the filesystem.
    /// </summary>
    /// <param name="session">
    ///   Session instance to save to the specified path.
    /// </param>
    public void Save(Session session)
    {
      Save(session, _path);
    }

    /// <summary>
    ///   Loads the summary object from the specified path.
    /// </summary>
    public Session Load()
    {
      return Load<Session>(_path);
    }
  }
}