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
using System.IO;
using SPV3.Bbkpify;
using YuMi.Output;

namespace SPV3.Unbbkpify.CLI
{
    /// <summary>
    ///     Main program class.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     Console program entry.
        /// </summary>
        /// <param name="args">
        ///     args[0]: BBKP bitmaps directory path
        /// </param>
        internal static void Main(string[] args)
        {
            ShowBanner();

            var directoryPath = string.Empty;

            if (args.Length < 1)
            {
                Line.Write("Not enough arguments provided. Using manual input...", ConsoleColor.Yellow, "WARN");

                Console.ForegroundColor = ConsoleColor.Cyan;
                while (DirectoryValidator.GetStatus(directoryPath) != DirectoryStatus.IsValid)
                {
                    Line.Write("Please input a valid directory path:", ConsoleColor.Cyan, "STEP");
                    directoryPath = Console.ReadLine();
                }
            }
            else
            {
                directoryPath = args[0];

                if (DirectoryValidator.GetStatus(directoryPath) == DirectoryStatus.DoesNotExist)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("Target folder does not exist.");
                    Environment.Exit((int) ExitCodes.InvalidFilesFolderPath);
                }
            }

            var files = Directory.GetFiles(directoryPath, $"*.{Bbkpify.Main.Extension}*", SearchOption.AllDirectories);

            try
            {
                Bbkpify.Main.ResetBitmapFiles(files);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(e.Message);
                Environment.Exit((int) ExitCodes.ExceptionHasBeenThrown);
            }

            Line.Write($"\nRestored bitmaps in '{directoryPath}'!", ConsoleColor.Green, "DONE");
            Console.ReadLine();
            Environment.Exit((int) ExitCodes.Success);
        }

        /// <summary>
        ///     Outputs the main ASCII banner.
        /// </summary>
        private static void ShowBanner()
        {
            Line.Write(Ascii.Banner, ConsoleColor.Magenta);
            Line.Write($@"
Usage: .\{AppDomain.CurrentDomain.FriendlyName} <1>
         1 - Directory name to undo the bbkpify process in (e.g. '.\cmt\tags', 'C:\cmt\tags')
", ConsoleColor.Cyan);
        }
    }
}