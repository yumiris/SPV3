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

namespace HXE
{
  /// <summary>
  ///   Object for permitting enum-identified process existing.
  /// </summary>
  public static class Exit
  {
    /// <summary>
    ///   Available exit codes for HXE.
    /// </summary>
    public enum Code
    {
      Success         = 0,
      InvalidCommand  = Success         + 1,
      InvalidArgument = InvalidCommand  + 2,
      Exception       = InvalidArgument + 4
    }

    /// <summary>
    ///   Wrapper for Environment.Exit with enum support.
    /// </summary>
    /// <param name="code">
    ///   Exit code represented by <see cref="Code" />.
    /// </param>
    public static void WithCode(Code code)
    {
      Exit((int) code);
    }
  }
}