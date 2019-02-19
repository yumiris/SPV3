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

namespace SPV3.Bbkpify.Infrastructure.Data
{
  /// <summary>
  ///   Repository which persists a Configuration object to a binary on the filesystem.
  /// </summary>
  public class ConfigurationRepository : Repository
  {
    /// <summary>
    ///   Default binary used for Configuration persistence.
    /// </summary>
    private const string Binary = "SPV3.Bbkpify.Configuration.bin";

    /// <summary>
    ///   Path for Configuration object data I/O.
    /// </summary>
    private readonly string path;

    /// <summary>
    ///   ConfigurationRepository constructor.
    /// </summary>
    /// <param name="path">
    ///   Path for Configuration object data I/O.
    /// </param>
    public ConfigurationRepository(string path = null)
    {
      if (path != null)
      {
        this.path = path;
        return;
      }

      var appData  = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      var fullPath = Path.Combine(appData, Binary);

      this.path = fullPath;
    }

    /// <summary>
    ///   Saves inbound Configuration object to the filesystem.
    /// </summary>
    /// <param name="configuration">
    ///   Configuration instance to save to the specified path.
    /// </param>
    public void Save(Configuration configuration)
    {
      Save(configuration, path);
    }

    /// <summary>
    ///   Loads the Configuration object from the specified path.
    /// </summary>
    public Configuration Load()
    {
      return Load<Configuration>(path);
    }
  }
}