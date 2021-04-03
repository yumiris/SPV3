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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.IO.Path;

namespace HXE.Steam
{
  public class Libraries : File
  {
    /// <summary>
    /// Create an object representing LibraryFolders.vdf on the file system.
    /// Local functions will read the contents of this file object.
    /// </summary>
    public File         LibFoldersVdf = (File) Paths.Steam.Libraries;
    public List<string> LibList       = new List<string>();  /// Arbitrary limit of 16 libraries. If you go over this, you're insane.
    public List<string> ReturnPaths   = new List<string>(); /// Arbitrary limit of 16 results per search.

    /// <summary>
    /// Read Steam's "libraryfolders.vdf" and assign the library folders to an index array.
    /// </summary>
    public void ParseLibraries()
    {
      if (!LibFoldersVdf.Exists() || LibFoldersVdf.Path == null)
        throw new FileNotFoundException("Steam Library list not found.");

      try
      {
        var text = LibFoldersVdf.ReadAllText();

        List<string> libs = text.Split(new char[] { '\"' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
        libs = libs.Where(line => line.Contains(":")).ToList();
        foreach(string line in libs)
        {
          LibList.Add(line.Replace("\\\\", "\\"));
        }
      }
      catch (System.Exception e)
      {
        throw new System.Exception("Failed to Parse Steam Libraries file", e);
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
    /// Search each discovered Steam Library for a given path. <br/>
    /// Each result is assigned to the ReturnPaths array.
    /// </summary>
    /// <param name="pathInLibrary">
    /// The path to find within the Steam Library folders.
    /// </param>
    /// <remarks>
    /// If only one result is expected, it can be accessed as ReturnPath[0]. <br/>
    /// If there are multiple results, implement another While loop to filter them.
    /// </remarks>
    public List<string> FindInLibraries(string pathInLibrary)
    {
      List<string> list = new List<string>();
      try
      {
        foreach(string library in LibList)
        {
          string path = Combine(library, "steamapps", "common", pathInLibrary);
          if(System.IO.File.Exists(path))
            list.Add(path);
        }
      }
      catch (System.Exception e)
      {
        throw new System.Exception($"Failed to find {pathInLibrary} in Steam Libraries.", e);
      }

      if(list.Count == 0)
        throw new FileNotFoundException($"Failed to find {Path} in Steam Libraries.");

      return list;
    }
  }
}