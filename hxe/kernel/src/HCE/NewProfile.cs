using System.IO;
using HXE.HCE;
using static System.Environment;
using static HXE.Console;
using static HXE.Paths;
using Directory = System.IO.Directory;

namespace HXE
{
  /// <inheritdoc />
  /// <summary>
  ///   Object used for creating a new Player Profile.
  /// </summary>
  class NewProfile
  {
    /// <summary>
    ///   Generate a Player Profile and assign it as the LastProfile.
    /// </summary>
    /// <param name="scaffold"    >Inherit and pass the bool indicating if the scaffold must be created.</param>
    /// <param name="path"        >Inherit and pass the -path parameter.</param>
    /// <param name="lastprofile" >Inherit the LastProfile instance.</param>
    /// <param name="profile"     >Inherit the Profile instance.</param>
    public static void Generate(string path, LastProfile lastprofile, Profile profile, bool scaffold)
    {
      Core("Beginning to generate a new Player Profile");
      /// todo:
      ///   populate with default settings
      ///     blam.sav has some defaults listed. What needs to be set manually?
      profile.Path = Custom.Profile(path, profile.Details.Name);
      lastprofile.Profile = profile.Details.Name;

      if (!scaffold)
        Scaffold(path, lastprofile, profile);
      else
      {
        profile.Save();
        lastprofile.Save();
      }
    }

    /// <summary>
    ///   Output the File.Path variable's current value. Verify its full path exists in the file system.
    /// </summary>
    public static void VerifyPath(string path)
    {
      bool isDir = System.IO.File.GetAttributes(path).HasFlag(FileAttributes.Directory);

      Debug(NewLine
        + NewLine + $"Path is currently \"{path}\""
        + NewLine + $"The Full Path is \"{Path.GetFullPath(path)}\""
        + NewLine + $"Path points to a folder? {isDir}"
        + NewLine + $"\"{path}\" exists?" + (isDir? Directory.Exists(path) : System.IO.File.Exists(path))
        );
    }

    /// <summary>
    ///   Create file and folder structure for Profile.Generate() using variables from GenVars.
    /// </summary>
    /// <param name="path"    > -path parameter to pass to Halo and write to profiles.</param>
    /// <param name="profile" > Object to represent as string.</param>
    /// <param name="file"    > An instance of the File class</param>
    public static void Scaffold(string path, LastProfile lastprofile, Profile profile)
    {
      Core("Creating Scaffold...");

      /** Scaffold Root
       * Set Path to the -path parameter
       * If it doesn't exist, create it.
       * Print the full path to the console.
       * e.g. ".\temp\"
       */
      var file = (File) path;
      Directory.CreateDirectory(file.Path);
      VerifyPath(file.Path);

      /** Savegames
       * Set Path to the savegames directory
       * If it doesn't exist, create it.
       * e.g. ".\temp\savegames\
       */
      file.Path = Custom.Profiles(path);
      Directory.CreateDirectory(file.Path);
      VerifyPath(file.Path);

      /** Profile's folder
       * Set Path to the current Profile's directory, et cetera
       * If it doesn't exist, create it.
       */
      file.Path = Custom.ProfileDirectory(path, profile.Details.Name);  /// e.g. ".\temp\savegames\New001\"
      Directory.CreateDirectory(file.Path);
      VerifyPath(file.Path);

      /** Profile's Blam.sav
       * Create blam.sav
       * Write Input Blanks
       *
       * <see cref="Profile.Offset"/>
       */
      using (var fs = new FileStream(Custom.Profile(path, profile.Details.Name), FileMode.OpenOrCreate)) /// e.g. ".\temp\savegames\New001\blam.sav"
      using (var ms = new MemoryStream(8192))
      using (var bw = new BinaryWriter(ms))
      {
        fs.Position = 0;
        fs.CopyTo(ms);

        /** Write Bindings Buffer (7fff) */
        {
          ms.Position = 0x13c;
          while (ms.Position < 0x938)
          {
            bw.Write((ushort) 0x7fff);
          }
        }

        /** Unknown */
        ms.Position = 0x12e;
        bw.Write((byte) 0x3);

        ms.Position = 0;
        ms.CopyTo(fs);
      }
      profile.Save();
      VerifyPath(Custom.Profile(path, profile.Details.Name));

      /** Savegame.bin
       * Create savegame.bin
       * e.g. ".\temp\savegames\New001\savegame.bin"
       */
      file.Path = Custom.Progress(path, profile.Details.Name);
      file.WriteAllBytes(new byte[0x480000]); /// 0x480000 == int 4718592
      VerifyPath(file.Path);

      /** Profile file (Waypoint)
       * Create waypoint
       * e.g. ".\temp\savegames\New001\New001"
       */
      file.Path = Custom.Waypoint(path, profile.Details.Name);
      file.WriteAllText(Custom.Waypoint(path, profile.Details.Name));
      VerifyPath(file.Path);

      /** lastprof.txt
       * Create or overwrite lastprof.txt
       * e.g. ".\temp\lastprof.txt"
       */
      file.Path = Custom.LastProfile(path);
      lastprofile.Path = file.Path;
      lastprofile.Name = profile.Details.Name;
      lastprofile.Save();
      VerifyPath(file.Path);

      Info("Scaffold Completed.");
    }
  }
}
