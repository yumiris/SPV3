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

﻿using System;

namespace YuMi.Output
{
    /// <summary>
    /// Wrapper class for System.Console for pretty output.
    /// </summary>
    public static class Line
    {
        /// <summary>
        /// Writes to the Console.
        /// </summary>
        /// <param name="message">Message to write.</param>
        public static void Write(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Writes to the Console with a given foreground colour.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="colour">Message foreground colour.</param>
        public static void Write(string message, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Write(message);
        }
        
        /// <summary>
        /// Writes to the Console with a given foreground colour and prefix at the start.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="colour">Message foreground colour.</param>
        /// <param name="prefix">Prefixes such as WARN, INFO, HALT, STEP, DONE.</param>
        public static void Write(string message, ConsoleColor colour, string prefix)
        {
            Console.ForegroundColor = colour;
            Write($"[ {prefix} ] | {message}");
        }
        
        /// <summary>
        /// Writes to the Console with a given foreground colour, prefix at the start, and descriptive title.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="colour">Message foreground colour.</param>
        /// <param name="prefix">Prefixes such as WARN, INFO, HALT, STEP, DONE.</param>
        /// <param name="title">The process' or the step's name.</param>
        public static void Write(string message, ConsoleColor colour, string prefix, string title)
        {
            Console.ForegroundColor = colour;
            Write($"[ {prefix} ] | {title} | {message}");
        }
    }
}