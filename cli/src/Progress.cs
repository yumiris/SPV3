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

using System.IO;
using System.Text;

namespace SPV3.CLI
{
  /// <inheritdoc />
  /// <summary>
  ///   Object representation of a savegame.bin file on the filesystem.
  /// </summary>
  public class Progress : File
  {
    public Campaign.Mission    Mission    { get; set; } = Campaign.Mission.Spv3A10;
    public Campaign.Difficulty Difficulty { get; set; } = Campaign.Difficulty.Normal;

    /// <summary>
    ///   Loads object state from the inbound file.
    /// </summary>
    public void Load()
    {
      /**
       * Infers the difficulty and returns the Campaign.Difficulty representation.
       */

      Campaign.Difficulty GetDifficulty(BinaryReader reader)
      {
        reader.BaseStream.Seek(0x1E2, SeekOrigin.Begin);

        switch (reader.ReadByte())
        {
          case 0x0:
            return Campaign.Difficulty.Noble;
          case 0x1:
            return Campaign.Difficulty.Normal;
          case 0x2:
            return Campaign.Difficulty.Heroic;
          case 0x3:
            return Campaign.Difficulty.Legendary;
          default:
            return Campaign.Difficulty.Normal;
        }
      }

      /**
       * Infers the difficulty and returns the Campaign.Difficulty mission.
       */

      Campaign.Mission GetMission(BinaryReader reader)
      {
        var bytes = new byte[32];

        reader.BaseStream.Seek(0x1E8, SeekOrigin.Begin);
        reader.BaseStream.Read(bytes, 0, 32);

        switch (Encoding.UTF8.GetString(bytes).TrimEnd('\0'))
        {
          case "spv3a10":
            return Campaign.Mission.Spv3A10;
          case "spv3a30":
            return Campaign.Mission.Spv3A30;
          case "spv3a50":
            return Campaign.Mission.Spv3A50;
          case "spv3b30":
            return Campaign.Mission.Spv3B30;
          case "spv3b30_evolved":
            return Campaign.Mission.Spv3B30Evolved;
          case "spv3b40":
            return Campaign.Mission.Spv3B40;
          case "spv3c10":
            return Campaign.Mission.Spv3C10;
          case "spv3c20":
            return Campaign.Mission.Spv3C20;
          case "spv3c40":
            return Campaign.Mission.Spv3C40;
          case "spv3d20":
            return Campaign.Mission.Spv3D20;
          case "spv3d25":
            return Campaign.Mission.Spv3D25;
          case "spv3d30":
            return Campaign.Mission.Spv3D30;
          case "spv3d30_evolved":
            return Campaign.Mission.Spv3D30Evolved;
          case "spv3d40":
            return Campaign.Mission.Spv3D40;
          case "lumoria_a":
            return Campaign.Mission.LumoriaA;
          case "lumoria_b":
            return Campaign.Mission.LumoriaB;
          case "lumoria_cd":
            return Campaign.Mission.LumoriaCd;
          default:
            return Campaign.Mission.Spv3A10;
        }
      }

      using (var reader = new BinaryReader(System.IO.File.Open(Path, FileMode.Open)))
      {
        Difficulty = GetDifficulty(reader);
        Mission    = GetMission(reader);
      }
    }

    /// <summary>
    ///   Represents the inbound object as a string.
    /// </summary>
    /// <param name="progress">
    ///   Object to represent as string.
    /// </param>
    /// <returns>
    ///   String representation of the inbound object.
    /// </returns>
    public static implicit operator string(Progress progress)
    {
      return progress.Path;
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
    public static explicit operator Progress(string name)
    {
      return new Progress
      {
        Path = name
      };
    }
  }
}