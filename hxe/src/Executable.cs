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
using System.Text;
using Microsoft.Win32;
using static System.Environment;
using static System.IO.File;
using static System.IO.Path;
using static HXE.Console;
using static Microsoft.Win32.RegistryKey;

namespace HXE
{
  /// <inheritdoc />
  /// <summary>
  ///   Representation of the HCE executable on the filesystem.
  /// </summary>
  public class Executable : File
  {
    public VideoOptions   Video   { get; set; } = new VideoOptions();
    public DebugOptions   Debug   { get; set; } = new DebugOptions();
    public ProfileOptions Profile { get; set; } = new ProfileOptions();

    public static Executable Detect()
    {
      const string hce = Paths.Files.Executable;

      /**
       * Detect based on the current directory.
       */

      {
        var currentPath = Combine(CurrentDirectory, hce);

        if (System.IO.File.Exists(currentPath))
          return (Executable) currentPath;
      }

      /**
       * Detect based on the default installation path.
       */

      {
        const string directory64   = @"C:\Program Files (x86)\Microsoft Games\Halo Custom Edition";
        var          defaultPath64 = Combine(directory64, hce);

        if (System.IO.File.Exists(defaultPath64))
          return (Executable) defaultPath64;

        const string directory32   = @"C:\Program Files\Microsoft Games\Halo Custom Edition";
        var          defaultPath32 = Combine(directory32, hce);

        if (System.IO.File.Exists(defaultPath32))
          return (Executable) defaultPath32;
      }

      /**
       * Detect based on registry key values.
       */

      {
        const string registryLocation = @"SOFTWARE\Microsoft\Microsoft Games\Halo CE";
        const string registryIdentity = @"EXE Path";

        using (var view = OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
        using (var key = view.OpenSubKey(registryLocation))
        {
          var path = key?.GetValue(registryIdentity);
          if (path != null)
          {
            var registryExe64 = $@"{path}\{hce}";

            if (System.IO.File.Exists(registryExe64))
              return (Executable) registryExe64;
          }
        }

        using (var view = OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
        using (var key = view.OpenSubKey(registryLocation))
        {
          var path = key?.GetValue(registryIdentity);
          if (path != null)
          {
            var registryExe32 = $@"{path}\{hce}";

            if (System.IO.File.Exists(registryExe32))
              return (Executable) registryExe32;
          }
        }
      }

      /**
       * Detect based on SPV3 installation path.
       */

      {
        if (!System.IO.File.Exists(Paths.Files.Installation))
          throw new FileNotFoundException("Could not detect executable on the filesystem.");

        var spv3exe = Combine(System.IO.File.ReadAllText(Paths.Files.Installation).TrimEnd('\n'), hce);

        if (System.IO.File.Exists(spv3exe))
          return (Executable) spv3exe;
      }

      throw new FileNotFoundException("Could not detect executable on the filesystem.");
    }

    /// <summary>
    ///   Invokes the HCE executable with the arguments that represent this object's properties' states.
    /// </summary>
    public void Start()
    {
      /**
       * Converts the properties to arguments which the HCE executable can be invoked with.
       */
      string GetArguments()
      {
        var args = new StringBuilder();

        /**
         * Arguments for debugging purposes. 
         */
        if (Debug.Console)
          ApplyArgument(args, "-console ");

        if (Debug.Developer)
          ApplyArgument(args, "-devmode ");

        if (Debug.Screenshot)
          ApplyArgument(args, "-screenshot ");

        /**
         * Arguments for video overrides.
         */

        if (Video.Window)
          ApplyArgument(args, "-window ");

        if (Video.Width > 0 && Video.Height > 0 && Video.Refresh > 0)
          ApplyArgument(args, $"-vidmode {Video.Width},{Video.Height},{Video.Refresh} ");

        if (Video.Adapter > 1)
          ApplyArgument(args, $"-adapter {Video.Adapter} ");

        /**
         * Argument for custom profile path.
         */

        if (!string.IsNullOrWhiteSpace(Profile.Path))
          ApplyArgument(args, $"-path {GetFullPath(Profile.Path)} ");

        return args.ToString();
      }

      Info("Starting process for HCE executable");

      Process.Start(new ProcessStartInfo
      {
        FileName = Path,
        WorkingDirectory = GetDirectoryName(Path) ??
                           throw new DirectoryNotFoundException("Failed to infer process working directory."),
        Arguments = GetArguments()
      });

      Info("Successfully started HCE executable");
    }

    private static void ApplyArgument(StringBuilder args, string arg)
    {
      args.Append(arg);
      Debug("Appending argument: " + arg);
    }

    /// <summary>
    ///   Represents the inbound object as a string.
    /// </summary>
    /// <param name="executable">
    ///   Object to represent as string.
    /// </param>
    /// <returns>
    ///   String representation of the inbound object.
    /// </returns>
    public static implicit operator string(Executable executable)
    {
      return executable.Path;
    }

    /// <summary>
    ///   Represents the inbound string as an object.
    /// </summary>
    /// <param name="executable">
    ///   String to represent as object.
    /// </param>
    /// <returns>
    ///   Object representation of the inbound string.
    /// </returns>
    public static explicit operator Executable(string executable)
    {
      return new Executable
      {
        Path = executable
      };
    }

    public class DebugOptions
    {
      public bool Console    { get; set; }
      public bool Developer  { get; set; }
      public bool Screenshot { get; set; }
    }

    public class VideoOptions
    {
      public bool Window  { get; set; }
      public int  Width   { get; set; }
      public int  Height  { get; set; }
      public int  Refresh { get; set; }
      public int  Adapter { get; set; }
    }

    public class ProfileOptions
    {
      public string Path { get; set; } = Paths.Directories.HCE;
    }
  }
}