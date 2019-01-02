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
    /// <summary>
    ///     SPV3.Compiler.CLI main.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     Status object for the compiler.
        /// </summary>
        private static readonly Status Status = new Status();

        /// <summary>
        ///     ASCII banner for the console.
        /// </summary>
        private static string Banner => @"
-------------------------------------------------------------------------
~ SPV3 Compiler - (C) 2019 Emilian Roman - GNU General Public License 3 ~
-------------------------------------------------------------------------
";

        /// <summary>
        ///     Invokes the SPV3 Compiler with the given arguments.
        /// </summary>
        /// <param name="args">
        ///     [0] = Source directory.
        ///     [1] = Target directory.
        /// </param>
        public static void Main(string[] args)
        {
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

                Exit(Banner, 0);
            }
            catch (Exception exception)
            {
                Exit(exception.ToString(), 3);
            }
        }

        /// <summary>
        ///     Exits application with the provided message and exit code.
        /// </summary>
        /// <param name="message">
        ///     Pre-exit message to write to the console.
        /// </param>
        /// <param name="code">
        ///     0 = successful, anything else = failure.
        /// </param>
        private static void Exit(string message, int code)
        {
            Status.CommitStatus(message, code == 0 ? StatusType.Success : StatusType.Failure);
            Environment.Exit(code);
        }
    }
}