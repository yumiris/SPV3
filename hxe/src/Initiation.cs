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

using System;
using System.Text;
using static HXE.Configuration.PostProcessingConfiguration;
using static HXE.Configuration.PostProcessingConfiguration.DofOptions;
using static HXE.Configuration.PostProcessingConfiguration.MxaoOptions;

namespace HXE
{
  /// <inheritdoc />
  /// <summary>
  ///   Object representation of the OpenSauce initc.txt file on the filesystem.
  /// </summary>
  public class Initiation : File
  {
    public bool       CinematicBars   { get; set; } = true;
    public bool       PlayerAutoaim   { get; set; } = true;
    public bool       PlayerMagnetism { get; set; } = true;
    public Campaign.Mission    Mission         { get; set; } = Campaign.Mission.Spv3A10;
    public Campaign.Difficulty Difficulty      { get; set; } = Campaign.Difficulty.Normal;

    public Configuration.PostProcessingConfiguration PostProcessing { get; set; } =
      new Configuration.PostProcessingConfiguration();

    /// <summary>
    ///   Saves object state to the inbound file.
    /// </summary>
    public void Save()
    {
      /**
       * Converts the Campaign.Difficulty value to a game_difficulty_set parameter, as specified in the loader.txt
       * documentation.
       */
      string GetDifficulty()
      {
        switch (Difficulty)
        {
          case Campaign.Difficulty.Normal:
            return "normal";
          case Campaign.Difficulty.Heroic:
            return "hard";
          case Campaign.Difficulty.Legendary:
            return "impossible";
          case Campaign.Difficulty.Noble:
            return "easy";
          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      /**
       * Encodes formal permutations to an integer. The permutations and values are specified in the doc/shaders.txt
       * documentation.
       */

      int GetPostProcessing()
      {
        var mxao = PostProcessing.Mxao;
        var dof  = PostProcessing.Dof;
        var mb   = PostProcessing.MotionBlur;
        var lf   = PostProcessing.DynamicLensFlares;
        var vol  = PostProcessing.Volumetrics;
        var ld   = PostProcessing.LensDirt;

        if (mxao  == MxaoOptions.Off && dof == DofOptions.Off && mb == MotionBlurOptions.Off
            && lf == false           && vol == false          && ld)
          return 0;

        if (mxao  == MxaoOptions.Off && dof == DofOptions.Off && mb == MotionBlurOptions.Off
            && lf == false           && vol                   && ld)
          return 1;

        if (mxao  == MxaoOptions.Off && dof == DofOptions.Low && mb == MotionBlurOptions.BuiltIn
            && lf == false           && vol                   && ld)
          return 2;

        if (mxao  == MxaoOptions.Low && dof == DofOptions.Low && mb == MotionBlurOptions.BuiltIn
            && lf == false           && vol                   && ld)
          return 3;

        if (mxao == MxaoOptions.Low && dof == DofOptions.Low && mb == MotionBlurOptions.PombLow
            && lf                   && vol                   && ld)
          return 4;

        if (mxao == MxaoOptions.Low && dof == DofOptions.High && mb == MotionBlurOptions.PombLow
            && lf                   && vol                    && ld)
          return 5;

        if (mxao == MxaoOptions.High && dof == DofOptions.High && mb == MotionBlurOptions.PombLow
            && lf                    && vol                    && ld)
          return 6;

        if (mxao == MxaoOptions.High && dof == DofOptions.High && mb == MotionBlurOptions.PombHigh
            && lf                    && vol                    && ld)
          return 7;

        return 0;
      }

      var difficulty = GetDifficulty();
      var shaders    = GetPostProcessing();
      var mission    = (int) Mission;
      var autoaim    = PlayerAutoaim ? 1 : 0;
      var magnetism  = PlayerMagnetism ? 1 : 0;
      var cinematic  = CinematicBars ? 1 : 0;

      var output = new StringBuilder();
      output.AppendLine($"set f1 {shaders}");
      output.AppendLine($"set f3 {mission}");
      output.AppendLine($"set f5 {cinematic}");
      output.AppendLine($"player_autoaim {autoaim}");
      output.AppendLine($"player_magnetism {magnetism}");
      output.AppendLine($"game_difficulty_set {difficulty}");

      Console.Info("Saving initiation data to the initc.txt file");
      WriteAllText(output.ToString());
      Console.Info("Successfully applied initc.txt configurations");
      Console.Debug("Initiation data: \n\n" + ReadAllText());
    }

    /// <summary>
    ///   Represents the inbound object as a string.
    /// </summary>
    /// <param name="initiation">
    ///   Object to represent as string.
    /// </param>
    /// <returns>
    ///   String representation of the inbound object.
    /// </returns>
    public static implicit operator string(Initiation initiation)
    {
      return initiation.Path;
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
    public static explicit operator Initiation(string name)
    {
      return new Initiation
      {
        Path = name
      };
    }
  }
}