/**
 * Copyright (c) 2019 Emilian Roman
 * Copyright (c) 2020 Noah Sherwin
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

using System.IO;
using static HXE.Paths.MCC;
using static System.IO.Path;

namespace HXE.Steam
{
  public class Libraries : File
  {
    /// <summary>
    /// Create an object representing LibraryFolders.vdf on the file system.
    /// Local functions will read the contents of this file object.
    /// </summary>
    public File     LibFoldersVdf = (File)SteamLibList;
    public string[] LibList       = new string[15];  /// Arbitrary limit of 16 libraries. If you go over this, you're insane.
    public string[] ReturnPaths   = new string[15]; /// Arbitrary limit of 16 results per search.
    
    /// <summary>
    /// Read Steam's "libraryfolders.vdf" and assign the library folders to an index array.
    /// </summary>
    public void ParseLibraries()
    {
      if (!LibFoldersVdf.Exists() || LibFoldersVdf.Path == null)
        throw new FileNotFoundException("Steam Library list not found.");
      string allText = LibFoldersVdf.ReadAllText();
      string[] lines = allText.Split('\n');
      ushort x = 0; 
      ushort y = 1;
      ushort z = (ushort)lines.GetUpperBound(0);

      /// <example>
      /// 
      /// </example>

      while (lines[x] != null)
      {
        /// extract the path string from the line if the line matches a pattern.
        if (lines[x].Contains($"\"{y}\"")) /// e.g. "1" OR "2" OR "3"
        {
          lines[x] = lines[x].Trim('\t'); /// Trim Tab characters
          lines[x] = lines[x].Replace($"\"{y}\"", "");
          lines[x] = lines[x].Replace("\"", "");
          lines[x] = lines[x].Replace("\\\\", "\\");
          lines[x] = lines[x].Trim('\t');
          LibList[y - 1] = lines[x]; /// y-1 because 0 is unused. This moves all entries so the first one occupies LibList[0]
          y++;
        }
        if (x != z)
          x++;
        else lines[x] = null;
      }
    }

    /// <summary>
    /// Pass a file path to replace the inferred Libraries path. Then execute ParseLibraries().
    /// </summary>
    /// <param name="path">A non-standard path to LibraryFolderss.vdf.</param>
    /// <remarks>
    /// The inferred path will work 99% of the time. This will probably be removed later.
    /// </remarks>
    public void ParseLibraries(string path)
    {
      LibFoldersVdf.Path = path;
      ParseLibraries();
    }

    /// <summary>
    /// Search each discovered Steam Library for a given path. Each result is assigned to the ReturnPaths array. 
    /// </summary>
    /// <param name="path">The path(s) to find within the Steam Library folders.</param>
    /// <remarks>
    /// If only one result is expected, it can be accessed as ReturnPath[0].
    /// If there are multiple results, implement another While loop to filter them.
    /// 
    /// </remarks>
    public void ScanLibraries(string path)
    {
      int x = 0; /// LibList index
      int y = 0; /// ReturnPaths index
      int z = LibList.GetUpperBound(0);

      while (x != 15 && LibList[x] != null)
      {
        File file = (File)LibList[x]; /// each element in LibList[] is assigned as a File object's Path.
        file.Path = Combine(file.Path, path);
        if (file.Exists())
        {
          ReturnPaths[y] = file.Path;
          y++;
        }
        if (x != z)
          x++;
        else
          LibList[x] = null;
      }
    }
  }
}