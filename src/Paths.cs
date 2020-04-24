/**
 * Copyright (c) 2019 Emilian Roman
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.Path;

namespace SPV3
{
  public class Paths
  {
    public const string Compile    = "0xCOMPILE";
    public const string AmaiSosu   = "amaisosu.exe";
    public const string Executable = "spv3.exe";

    public static readonly string Directory     = Combine(GetFolderPath(ApplicationData), "SPV3");
    public static readonly string Initiation    = Combine(Directory,                      "initc.txt");
    public static readonly string Exception     = Combine(Directory,                      "exception.log");
    public static readonly string Configuration = Combine(Directory,                      "loader-0x01.bin");
    public static readonly string Kernel        = Combine(Directory,                      "kernel-0x01.bin");
    public static readonly string DOOM          = Combine(CurrentDirectory,               "doom.bin");
    public static readonly string Photo         = Combine(CurrentDirectory,               "photo.bin");
    public static readonly string Setup         = Combine(CurrentDirectory,               "setup.exe");
    public static readonly string HXE           = Combine(CurrentDirectory,               "hxe.exe");
    public static readonly string Readme        = Combine(CurrentDirectory,               "readme.pdf");
    public static readonly string Changelog     = Combine(CurrentDirectory,               "changelog.pdf");
    public static readonly string Credits       = Combine(CurrentDirectory,               "credits.pdf");

    public static string Packages(string target)
    {
      return Combine(target, "data");
    }
  }
}