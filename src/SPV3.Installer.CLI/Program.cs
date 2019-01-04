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
using System.IO;
using System.Threading.Tasks;
using SPV3.Domain;
using SPV3.Installer.Installers;
using Directory = SPV3.Domain.Directory;

namespace SPV3.Installer.CLI
{
    /// <summary>
    ///     SPV3.Installer.CLI main.
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
--------------------------------------------------------------------------
~ SPV3 Installer - (C) 2019 Emilian Roman - GNU General Public License 3 ~
--------------------------------------------------------------------------
";

        /// <summary>
        ///     Invokes the SPV3 Installer with the given arguments.
        /// </summary>
        /// <param name="args">
        ///     [0] = Target directory.
        /// </param>
        public static void Main(string[] args)
        {
            if (args.Length < 1) Exit("Not enough arguments!", 1);

            var target = (Directory) args[0];
            var backup = (Directory) Path.Combine(target, "SPV3-" + Guid.NewGuid());

            if (!System.IO.Directory.Exists(target)) Exit("Target does not exist.", 1);

            try
            {
                Console.Clear();

                Task.Run(() =>
                    {
                        var manifest = ManifestRepository.LoadDefault();

                        new MetaInstaller(target, backup, Status).Install(manifest);
                    })
                    .GetAwaiter().GetResult();

                Exit(Banner, 0);
            }
            catch (Exception exception)
            {
                Exit(exception.ToString(), 2);
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