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

using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.Environment;

namespace SPV3
{
  /// <summary>
  ///   Object representing the AmaiSosu installer.
  /// </summary>
  public class AmaiSosu
  {
    /// <summary>
    ///   AmaiSosu download path.
    /// </summary>
    public const string Address = "https://github.com/yumiris/AmaiSosu/releases/latest";

    /// <summary>
    ///   AmaiSosu executable path.
    /// </summary>
    public string Path { get; set; } = Directory.EnumerateFiles(
      CurrentDirectory,
      "amaisosu*.exe",
      SearchOption.TopDirectoryOnly
      ).First();

    public bool Exists()
    {
      return File.Exists(Path);
    }

    public void Execute()
    {
      Process.Start(Exists() ? Path : Address).WaitForExit();
    }

    /// <summary>
    ///   Represents the inbound object as a string.
    /// </summary>
    /// <param name="amaiSosu">
    ///   Object to represent as string.
    /// </param>
    /// <returns>
    ///   String representation of the inbound object.
    /// </returns>
    public static implicit operator string(AmaiSosu amaiSosu)
    {
      return amaiSosu.Path;
    }

    /// <summary>
    ///   Represents the inbound string as an object.
    /// </summary>
    /// <param name="path">
    ///   String to represent as object.
    /// </param>
    /// <returns>
    ///   Object representation of the inbound string.
    /// </returns>
    public static explicit operator AmaiSosu(string path)
    {
      return new AmaiSosu
      {
        Path = path
      };
    }
  }
}