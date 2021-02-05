using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.IO.Path;
using static HXE.Console;

namespace HXE
{
  public class Patcher
  {
    /// Read patches.crk to Patches list
    /// foreach containing exe:haloce.exe, get and apply requested patches.
    /// if a patch is no longer requested (enabled), restore the original value.
    /// Patches will be passed to and stored in Kernel via bit-wise integer.
    /// Later, some patches may be configured in SPV3 loader. Perhaps via The "Advanced" menu aka HXE's Configuration UserControl. Bring it full circle.
    public class PatchGroup
    {
      public string Name       { get; set; } = string.Empty;   /* Make large address aware */
      public string Executable { get; set; } = string.Empty;   /* haloce.exe               */
      public List<DataSet> DataSet = new List<DataSet>();
    }

    public class DataSet // 00000136: 0F 2F
    {
      public uint Offset   { get; set; }
      public byte Original { get; set; }
      public byte Patch    { get; set; }
    }

    public static List<PatchGroup> Patches = Reader(); // See Write() for tmp overrides

    public static List<PatchGroup> Reader() 
    {
      /* Get patches.crk resource from assembly resources */
      var file = Properties.Resources.patches.Split().ToList();
      foreach (var line in file)
      {
        if (line.ToString().StartsWith(";"))
          file.Remove(line);
      }

      /* Read strings from memory to...array? */

      /* Parse the data to members "PatchGroup" of list "Patches" */

      /* return list */
      return new List<PatchGroup>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cfg">HXE.Kernel.Configuration.Tweaks.Patches unsigned integer</param>
    /// <param name="exePath">Path to Halo executable</param>
    public void Write(uint cfg, string exePath)
    {
      var LAA = true;
      var DRM = (cfg & EXEP.DISABLE_DRM_AND_KEY_CHECKS) != 0;
      var patchlist = new List<DataSet>
      {
        new DataSet() { Offset = 0x136, Original = 0x0F, Patch = 0x2F } /* LAA */
      };

      if (DRM)
      {
        // just the most important ones for now
        patchlist.Add(new DataSet() { Offset = 0x144c2B, Original = 0x38, Patch = 0xEB });
        patchlist.Add(new DataSet() { Offset = 0x144c2C, Original = 0x18, Patch = 0x13 });
      }

      /* Temporary LAA, DRM */
      {
        using (var fs = new FileStream(exePath, FileMode.Open, FileAccess.ReadWrite))
        using (var ms = new MemoryStream(0x24B000))
        using (var bw = new BinaryWriter(ms))
        using (var br = new BinaryReader(ms))
        {
          byte value;
          foreach (var patch in patchlist)
          {
            ms.Position = 0;
            fs.Position = 0;
            fs.CopyTo(ms);

            ms.Position = patch.Offset;
            value = patch.Patch;

            if (br.ReadByte() != value)
            {
              ms.Position -= 1; /* restore position */
              bw.Write(value);  /* patch            */

              fs.Position = 0;
              ms.Position = 0;
              ms.CopyTo(fs);

              if (patch.Offset == 0x136)
                Info($"Applied LAA patch to the HCE executable");
              if (DRM)
                Info($"Applied Partial DRM patch to the HCE executable");
            }
            else
            {
              if (patch.Offset == 0x136)
                Info($"HCE executable already patched with LAA");
              if (DRM && patch.Offset != 0x136)
                Info($"HCE executable already patched with NoDRM");
            }
          }
        }
      }

      /* Flexible patcher */ /** This does not yet function, but it does't throw exceptions */
      using (var fs = new FileStream(exePath, FileMode.Open, FileAccess.ReadWrite))
      using (var ms = new MemoryStream(0x24B000))
      using (var bw = new BinaryWriter(ms))
      using (var br = new BinaryReader(ms))
      {
        var FilteredPatches = new List<PatchGroup>();
        foreach (var PatchGroup in Patches)
        {
          if (PatchGroup.Executable == "haloce.exe")
            FilteredPatches.Add(PatchGroup);
        }
        foreach (var PatchGroup in FilteredPatches)
        {
          foreach (var DataSet in PatchGroup.DataSet) // I hate this
          {

            byte value  = false ? DataSet.Patch : DataSet.Original;
            long offset = DataSet.Offset;
            ms.Position = 0;
            fs.Position = 0;
            fs.CopyTo(ms);

            ms.Position = offset;

            switch(PatchGroup.Name)
            {
              case "":
                break;
              default:
                break;
            }

            if (br.ReadByte() != value)
            {
              ms.Position -= 1; /* restore position */
              bw.Write(value);  /* patch            */

              fs.Position = 0;
              ms.Position = 0;
              ms.CopyTo(fs);

              Info($"Applied \"{PatchGroup.Name}\" patch to the HCE executable");
            }
            else
            {
              Info($"HCE executable already patched with \"{PatchGroup.Name}\"");
            }
          }
        }
      }
    }

    /// <summary>
    /// Offsets for bitwise operations
    /// </summary>
    public static class EXEP
    {
      public const uint ENABLE_LARGE_ADDRESS_AWARE = 1 << 0x00; // Increase max memory range from 2GiB to 4GiB.
      public const uint DISABLE_DRM_AND_KEY_CHECKS = 1 << 0x01; // Removes several DRM/key checks.
      public const uint BIND_SERVER_TO_0000        = 1 << 0x02; // Fixes LAN game discovery. Helps systems with multiple interfaces. Can still bind to IP using `-ip` flag.
      public const uint DISABLE_SAFEMODE_PROMPT    = 1 << 0x03; // Disables prompt to restart Halo with disfunctional Safe Mode.
      public const uint DISABLE_SYSTEM_GAMMA       = 1 << 0x04; // Disables system-wide modification of display gamma. Disables in-game gamma altogether. Removes need for RegKeys.
      public const uint DISABLE_EULA               = 1 << 0x05; // Allows the game to be run without displaying the EULA. Removes the need for eula.dll to be present.
      public const uint DISABLE_REG_EXIT_STATE     = 1 << 0x06; // Makes the executable more portable by not requiring/adding registry keys.
      public const uint DISABLE_VEHICLE_AUTOCENTER = 1 << 0x07; // In stock Halo, the crosshair in vehicles (e.g. scorpion tank) will slowly move towards the horizon as you move.
      public const uint DISABLE_MOUSE_ACCELERATION = 1 << 0x08; // Self-explanatory.
      public const uint BLOCK_UPDATE_CHECKS        = 1 << 0x09; // Prevents checking for game updates.
      public const uint PREVENT_DESCOPING_ON_DMG   = 1 << 0x10; // Prevents zoomed-in weapons from descoping when the player takes damage.
    //public const uint ADD_TAG                    = 1 << 0x11; // pR0Ps' signature
    }
  }
}
