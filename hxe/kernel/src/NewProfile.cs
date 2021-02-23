using System.IO;
using HXE.HCE;
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
      ///   create the file.
      ///     double check it
      ///   DONE: if the file is still null, *then* throw an exception.
      ///     Do this everywhere this function is called.
      ///   populate with default settings
      ///     blam.sav has some defaults listed. What needs to be set manually?
      ///   load the file
      ///     Do I still need to do this?
      profile.Path = Custom.Profile(path, profile.Details.Name);
      lastprofile.Profile = profile.Details.Name;

      if (!scaffold)
        Scaffold(path, lastprofile, profile);

      profile.Save();
      lastprofile.Save();
    }

    /// <summary>
    ///   Output the File.Path variable's current value. Verify its full path exists in the file system.
    /// </summary>
    public static void VerifyPath(string path)
    {
      FileAttributes attr = System.IO.File.GetAttributes(path);
      Debug("");
      Debug($"Path is currently \"{path}\"");
      Debug($"The Full Path is \"{Path.GetFullPath(path)}\"");
      Debug($"Path is folder? {attr.HasFlag(FileAttributes.Directory)}");
      if (attr.HasFlag(FileAttributes.Directory)) 
      {
        Debug($"\"{path}\" exists? {Directory.Exists(path)}");
      }
      else
      {
        Debug($"\"{path}\" exists? {System.IO.File.Exists(path)}");
      }
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

      /// Set Path to the -path parameter
      /// Check if it exists, create it if it doesn't.
      /// Print the full path to the console.
      /// e.g. ".\temp\"
      var file = (File) path;
      Directory.CreateDirectory(file.Path);
      VerifyPath(file.Path);

      /// Set Path to the savegames directory, et cetera
      file.Path = Custom.Profiles(path);  /// e.g. ".\temp\savegames\
      Directory.CreateDirectory(file.Path);
      VerifyPath(file.Path);

      /// Set Path to the current Profile's directory, et cetera
      file.Path = Custom.ProfileDirectory(path, profile.Details.Name);  /// e.g. ".\temp\savegames\New001\"
      Directory.CreateDirectory(file.Path);
      VerifyPath(file.Path);

      /// Create blam.sav
      file.Path = Custom.Profile(path, profile.Details.Name); /// e.g. ".\temp\savegames\New001\blam.sav"
      file.WriteAllBytes(new byte[8192]);
      VerifyPath(file.Path);

      /// Create savegame.bin
      file.Path = Custom.Progress(path, profile.Details.Name); /// e.g. ".\temp\savegames\New001\savegame.bin"
      file.WriteAllBytes(new byte[0x480000]); /// 0x480000 == int 4718592
      VerifyPath(file.Path);

      /// Create waypoint
      file.Path = Custom.Waypoint(path, profile.Details.Name); /// e.g. ".\temp\savegames\New001\New001"
      file.WriteAllText(Custom.Waypoint(path, profile.Details.Name));
      VerifyPath(file.Path);
      /// "waypoint" is used to refer to the file at "Path\\savegames\\Name\\Name"
      /// Halo writes the path of this file as the file's contents.
      /// For instance, `.\profiles\savegames\New001\New001`
      ///   when `-path .\profiles`

      /// Create or overwrite lastprof.txt
      file.Path = Custom.LastProfile(path);
      file.WriteAllBytes(new byte[0xFF]); /// 255 int. Makes room for up to 255 characters.
      lastprofile.Path = file.Path;
      lastprofile.Save();
      VerifyPath(file.Path);

      Info("Scaffold created.");
    }
  }
}
