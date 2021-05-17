/**
 * Copyright (c) 2019 Emilian Roman
 * Copyright (c) 2021 Noah Sherwin
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
//using static System.IO.Path;

namespace HXE.Common
{
    public static class ExtPath
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="path">The path to sanitize.</param>
        /// <param name="doubleEscape">If true, escape backslash characters. Useful when writing to .reg files.</param>
        /// <returns>The sanitized path.</returns>
        public static string SanitizeSeparators(string path, bool doubleEscape = false)
        {
            path = System.IO.Path.GetFullPath(path);

            /** Return null if path-string is not a path */
            if (!path.Contains(@"\") && !path.Contains(@"/"))
                return "";

            /** Replace Forward Slash with Escaped Backslash */
            if (path.Contains("/"))
                path = path.Replace("/", "\\");

            /** Remove Path Separators, break down into substrings */
            string[] substrings = path.Split(new char[] { '\\' }, System.StringSplitOptions.RemoveEmptyEntries);

            /** Add Sanitized Path Separators */
            string result = "";
            for (int i = 0; i != substrings.Length; i++)
            {
                if (i == 0)
                {
                    result = substrings[0];
                    i++;
                }
                if (i != substrings.Length)
                    result = result + $"{System.IO.Path.DirectorySeparatorChar}" + substrings[i];
            }

            /** {OPTIONAL} Add escaped double-backslashes */
            if (doubleEscape)
                result = result.Replace("\\", @"\\");

            return result;
        }
    }
}