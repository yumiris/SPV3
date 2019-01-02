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

namespace SPV3.Compiler.CLI
{
    public class Status : IStatus
    {
        public void CommitStatus(string data)
        {
            CommitStatus(data, ConsoleColor.Cyan);
        }

        public void CommitStatus(string data, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(data);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}