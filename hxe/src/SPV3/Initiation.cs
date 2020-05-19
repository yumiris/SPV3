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
using static System.IO.File;
using static HXE.Console;
using static HXE.SPV3.PostProcessing;

namespace HXE.SPV3
{
  /// <inheritdoc />
  /// <summary>
  ///   Object representation of the OpenSauce initc.txt file on the filesystem.
  /// </summary>
  public class Initiation : File
  {
    public bool                CinematicBars     { get; set; } = true;
    public bool                PlayerAutoaim     { get; set; } = true;
    public bool                PlayerMagnetism   { get; set; } = true;
    public bool                MotionSensor      { get; set; } = true;
    public bool                MouseAcceleration { get; set; } = false;
    public int                 Gamma             { get; set; } = 0;
    public double              Speed             { get; set; } = 1;
    public bool                Unload            { get; set; } = false;
    public Campaign.Mission    Mission           { get; set; } = Campaign.Mission.Spv3A10;
    public Campaign.Difficulty Difficulty        { get; set; } = Campaign.Difficulty.Normal;
    public bool                Unlock            { get; set; }

    public PostProcessing PostProcessing { get; set; } =
      new PostProcessing();

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
       * The SPV3 campaign maps each have an ID assigned to them. SPV3.3 breaks the compatibility by incrementing all of
       * the IDs.
       *
       * To handle permit backwards compatibility with 3.2 and below, we conditionally decrement the mission ID that
       * will be written to the initiation file.
       *
       * The decrementation is determined by the presence of the Paths.SPV33 file in the working directory. If the file
       * is not found, then it's possible that this loader is being used on SPV3.2 and below, and thus we should use the
       * old (decremented) mission IDs.
       */
      int GetMission()
      {
        return System.IO.File.Exists(Paths.Version)
          ? (int) Mission      /* compatibility with >=SPV3.3 */
          : (int) Mission - 1; /* compatibility with <=SPV3.2 */
      }

      var mission      = GetMission();
      var difficulty   = GetDifficulty();
      var autoaim      = PlayerAutoaim ? 1 : 0;
      var magnetism    = PlayerMagnetism ? 1 : 0;
      var cinematic    = CinematicBars ? 0 : 1;
      var motionSensor = MotionSensor ? 1 : 0;
      var acceleration = MouseAcceleration ? 1 : 0;
      var gamma        = Gamma;
      var speed        = Speed;

      var output = new StringBuilder();
      output.AppendLine($"set f3 {mission}");
      output.AppendLine($"set loud_dialog_hack {cinematic}");
      output.AppendLine($"player_autoaim {autoaim}");
      output.AppendLine($"player_magnetism {magnetism}");
      output.AppendLine($"game_difficulty_set {difficulty}");
      output.AppendLine($"mouse_acceleration {acceleration}");
      output.AppendLine($"set rasterizer_hud_motion_sensor {motionSensor}");

      if (Unlock)
        output.Append("set f1 8");

      if (Gamma > 0)
        output.AppendLine($"set_gamma {gamma}");

      if (Speed > 1)
        output.AppendLine($"game_speed {speed}");

      if (Unload)
        output.AppendLine("pp_unload");

      /**
       * Encodes post-processing settings to the initc file. Refer to doc/shaders.txt for further information.
       */

      var mb   = PostProcessing.MotionBlur;
      var mxao = PostProcessing.MXAO;
      var dof  = PostProcessing.DOF;
      var vl   = PostProcessing.VolumetricLighting;
      var df   = PostProcessing.DynamicLensFlares;
      var ld   = PostProcessing.LensDirt;
      var fg   = PostProcessing.FilmGrain;
      var hv   = PostProcessing.HudVisor;
      var ssr  = PostProcessing.SSR;

      /* motion blur */
      {
        string option;

        switch (mb)
        {
          case MotionBlurOptions.Off:
            option = "1.0";
            break;
          case MotionBlurOptions.BuiltIn:
            option = "1.1";
            break;
          case MotionBlurOptions.PombLow:
            option = "1.2";
            break;
          case MotionBlurOptions.PombHigh:
            option = "1.3";
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }

        output.AppendLine($"set multiplayer_hit_sound_volume {option}");
      }

      /* mxao */
      {
        string option;

        switch (mxao)
        {
          case MxaoOptions.Off:
            option = "2";
            break;
          case MxaoOptions.Low:
            option = "3";
            break;
          case MxaoOptions.High:
            option = "4";
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }

        output.AppendLine($"set cl_remote_player_action_queue_limit {option}");
      }

      /* depth of field */
      {
        string option;

        switch (dof)
        {
          case DofOptions.Off:
            option = "6";
            break;
          case DofOptions.Low:
            option = "7";
            break;
          case DofOptions.High:
            option = "8";
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }

        output.AppendLine($"set cl_remote_player_action_queue_tick_limit {option}");
      }

      output.AppendLine("set rasterizer_soft_filter "                 + (vl ? "true" : "false"));  /* volumetrics    */
      output.AppendLine("set display_precache_progress "              + (df ? "true" : "false"));  /* dynamic flares */
      output.AppendLine("set use_super_remote_players_action_update " + (ld ? "false" : "true"));  /* lens dirt      */
      output.AppendLine("set use_new_vehicle_update_scheme "          + (fg ? "false" : "true"));  /* film grain     */
      output.AppendLine("set multiplayer_draw_teammates_names "       + (hv ? "false" : "true"));  /* hud visor      */
      output.AppendLine("set error_suppress_all "                     + (ssr ? "true" : "false")); /* ssr            */

      Info("Saving initiation data to the initc.txt file");
      WriteAllText(output.ToString());
      Info("Successfully applied initc.txt configurations");
      Debug("Initiation data: \n\n" + ReadAllText());
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