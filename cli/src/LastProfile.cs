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

namespace SPV3.CLI
{
  /// <inheritdoc />
  /// <summary>
  ///   Object representation of a lastprof.txt file.
  /// </summary>
  public class LastProfile : File
  {
    /// <summary>
    ///   Last accessed HCE profile.
    /// </summary>
    public string Profile { get; set; }

    /// <summary>
    ///   Interprets the profile name from the lastprof.txt file.
    /// </summary>
    public void Load()
    {
      var data   = ReadAllText();
      var split  = data.Split('\\');
      var offset = split.Length - 2;

      Profile = split[offset];
    }

    /// <summary>
    ///   Represents the inbound object as a string.
    /// </summary>
    /// <param name="lastProfile">
    ///   Object to represent as string.
    /// </param>
    /// <returns>
    ///   String representation of the inbound object.
    /// </returns>
    public static implicit operator string(LastProfile lastProfile)
    {
      return lastProfile.Path;
    }

    /// <summary>
    ///   Represents the inbound string as an object.
    /// </summary>
    /// <param name="name">
    ///   String to represent as object.
    /// </param>
    /// <returns>
    ///   Object representation of the inbound string.
    /// </returns>
    public static explicit operator LastProfile(string name)
    {
      return new LastProfile
      {
        Path = name
      };
    }
  }
}