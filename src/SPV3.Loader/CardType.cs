/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Loader.
 * 
 * SPV3.Loader is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Loader is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Loader.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace SPV3.Loader
{
    /// <summary>
    ///     Represents the card (GPU) types that HCE should be forced run as.
    /// </summary>
    public enum CardType
    {
        /// <summary>
        ///     Forces the game run in its default mode.
        /// </summary>
        Default,

        /// <summary>
        ///     Forces the game to run as a fixed function card.
        /// </summary>
        FixedFunction,

        /// <summary>
        ///     Forces the game to run as a shader 1.1 card.
        /// </summary>
        Shaders11Card,

        /// <summary>
        ///     Forces the game to run as a shader 1.4 card.
        /// </summary>
        Shaders14Card,

        /// <summary>
        ///     Forces the game to run as a shader 2.0 card.
        /// </summary>
        Shaders20Card
    }
}