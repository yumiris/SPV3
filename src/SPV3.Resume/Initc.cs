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

namespace SPV3.Resume
{
    /// <summary>
    ///   Type representing Initc campaign-related values.
    /// </summary>
    public class Initc
    {
        /// <summary>
        ///     Campaign mission.
        /// </summary>
        public Mission Mission { get; set; }

        /// <summary>
        ///     Campaign difficulty.
        /// </summary>
        public Difficulty Difficulty { get; set; }
    }
}