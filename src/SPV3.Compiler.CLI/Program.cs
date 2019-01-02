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
using System.Threading.Tasks;
using SPV3.Compiler.Compilers;
using SPV3.Compiler.Compressors;
using SPV3.Domain;

namespace SPV3.Compiler.CLI
{
    internal class Program
    {
        private static readonly Status Status = new Status();

        private static string Banner => @"
--------------------------------------------------------------------------------
~ SPV3 Compiler - v0.2 - (C) 2019 Emilian Roman - GNU General Public License 3 ~
--------------------------------------------------------------------------------
";

        public static void Main(string[] args)
        {
            Status.CommitStatus(Banner);

            if (args.Length < 2) Exit("Not enough arguments!", 1);

            var source = (Directory) args[0];
            var target = (Directory) args[1];

            if (!System.IO.Directory.Exists(source)) Exit("Source does not exist.", 2);

            if (!System.IO.Directory.Exists(target)) Exit("Target does not exist.", 2);

            try
            {
                Console.Clear();

                Task.Run(() =>
                    {
                        var compress = new InternalCompressor();
                        var compiler = new MetaCompiler(compress, Status);

                        compiler.Compile(source, target);
                    })
                    .GetAwaiter().GetResult();

                Console.ReadLine();
            }
            catch (Exception exception)
            {
                Exit(exception.ToString(), 3);
            }
        }

        private static void Exit(string message, int code)
        {
            Status.CommitStatus(message, code == 0 ? ConsoleColor.Green : ConsoleColor.Red);
            Environment.Exit(code);
        }
    }
}