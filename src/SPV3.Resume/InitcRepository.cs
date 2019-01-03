/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Resume.
 * 
 * SPV3.Resume is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Resume is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Resume.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using SPV3.Domain;

namespace SPV3.Resume
{
    /// <summary>
    ///     Saves data to the provided initc text file.
    /// </summary>
    public class InitcRepository
    {
        /// <summary>
        ///     Initc text file.
        /// </summary>
        private File _file;

        public InitcRepository(File file)
        {
            _file = file;
        }

        /// <summary>
        ///     Serialises Initc instance to the provided text file.
        /// </summary>
        /// <param name="progress">
        ///     Instance to serialise to the text file.
        /// </param>
        public void Save(Progress progress)
        {
            throw new NotImplementedException();
        }
    }
}