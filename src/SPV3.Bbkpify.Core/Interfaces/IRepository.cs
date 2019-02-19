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

namespace SPV3.Bbkpify.Core.Interfaces
{
  public interface IRepository
  {
    /// <summary>
    ///   Serialises the inbound object to a file at the given path.
    /// </summary>
    /// <param name="instance">
    ///   Object to persistently serialise.
    /// </param>
    /// <param name="path">
    ///   Path of the binary on the filesystem which should persistently store the serialised data.
    /// </param>
    void Save<T>(T instance, string path);

    /// <summary>
    ///   Deserialises the inbound object to an object.
    /// </summary>
    /// <param name="path">
    ///   Path to the file containing the serialised object.
    /// </param>
    /// <typeparam name="T">
    ///   Type of object to deserialise the data in the path to.
    /// </typeparam>
    /// <returns>
    ///   Object representation of the file specified in the inbound path.
    /// </returns>
    T Load<T>(string path);
  }
}