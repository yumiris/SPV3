/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Installer.
 * 
 * SPV3.Installer is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Installer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Installer.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using SPV3.Domain;

namespace SPV3.Installer.CLI
{
    /// <inheritdoc />
    public class Status : IStatus
    {
        /// <inheritdoc />
        public void CommitStatus(string data)
        {
            CommitStatus(data, ConsoleColor.Cyan);
        }

        /// <inheritdoc />
        public void CommitStatus(string data, StatusType type)
        {
            var symbol = string.Empty;
            var colour = ConsoleColor.White;

            switch (type)
            {
                case StatusType.Success:
                    symbol = "✔";
                    colour = ConsoleColor.Green;
                    break;
                case StatusType.Warning:
                    symbol = "⚠";
                    colour = ConsoleColor.Yellow;
                    break;
                case StatusType.Failure:
                    symbol = "⯃";
                    colour = ConsoleColor.Red;
                    break;
                case StatusType.Require:
                    symbol = "⌘";
                    colour = ConsoleColor.Cyan;
                    break;
            }

            CommitStatus($"{symbol} | {data}", colour);
        }

        /// <summary>
        ///     Sets console to the inbound colour and writes to it.
        /// </summary>
        /// <param name="data">
        ///     Data (status) to write to the console.
        /// </param>
        /// <param name="color">
        ///     Colour to set the console to for the data.
        /// </param>
        private void CommitStatus(string data, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(data);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}