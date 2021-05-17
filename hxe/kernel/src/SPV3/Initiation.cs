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

using System;
using System.Text;
using static HXE.Console;

namespace HXE.SPV3
{
    /// <inheritdoc />
    /// <summary>
    ///   Object representation of the OpenSauce initc.txt file on the filesystem.
    /// </summary>
    public class Initiation : File
    {
        public bool CinemaBars { get; set; } = false;
        public bool PlayerAutoaim { get; set; } = true;
        public bool PlayerMagnetism { get; set; } = true;
        public bool MotionSensor { get; set; } = true;
        public bool MouseAcceleration { get; set; } = false;
        public int Gamma { get; set; } = 0;
        public bool Unload { get; set; } = false;
        public Resume Resume { get; set; } = new Resume();
        public Progress Progress { get; set; } = new Progress();
        public bool Unlock { get; set; }
        public bool Attract { get; set; } = true;
        public uint Shaders { get; set; } = 0;

        /// <summary>
        ///   Saves object state to the inbound file.
        /// </summary>
        public void Save()
        {
            var autoaim = PlayerAutoaim ? 1 : 0;
            var magnetism = PlayerMagnetism ? 1 : 0;
            var cinemabars = CinemaBars ? 0 : 1;
            var motionSensor = MotionSensor ? 1 : 0;
            var acceleration = MouseAcceleration ? 1 : 0;
            var gamma = Gamma;

            var output = new StringBuilder();

            if (Resume.Enabled)
            {
                output.AppendLine("\n;;;  Set Mission/Progress");
                output.AppendLine($"set {Resume.Initiation} {Progress.Mission.Initiation}");

                output.AppendLine("\n;;;  Set Difficulty");
                output.AppendLine($"game_difficulty_set {Progress.Difficulty.Initiation}");
            }

            output.AppendLine("\n;;;  Toggle cinematic black bars");
            output.AppendLine($"set loud_dialog_hack {cinemabars}");

            output.AppendLine("\n;;;  Toggle projectile trajectory aim assist");
            output.AppendLine($"player_autoaim {autoaim}");

            output.AppendLine("\n;;;  Toggle reticle/aiming magnetism");
            output.AppendLine($"player_magnetism {magnetism}");

            output.AppendLine("\n;;;  Toggle mouse acceleration; Sometimes it works, sometimes it doesn't");
            output.AppendLine($"mouse_acceleration {acceleration}");

            output.AppendLine("\n;;;  Toggle Motion Sensor");
            output.AppendLine($"set rasterizer_hud_motion_sensor {motionSensor}");

            if (Unlock)
            {
                output.AppendLine("\n;;;  Unlock all levels");
                output.AppendLine("set f1 8");
            }

            if (Attract)
            {
                output.AppendLine("\n;;;  Play attractive film on startup");
                output.AppendLine("play_bink_movie attract.bik");
            }

            if (Gamma > 0)
            {
                output.AppendLine("\n;;;  Override system video gamma");
                output.AppendLine($"set_gamma {gamma}");
            }

            if (Unload)
            {
                output.AppendLine("\n;;;  Disable post-processing");
                output.AppendLine("pp_unload");
            }

            /**
             * Encodes post-processing settings to the initc file. Refer to doc/shaders.txt for further information.
             */

            output.AppendLine("\n;;;  Toggle Volumetric Lighting");
            output.AppendLine((Shaders & PP.VOLUMETRIC_LIGHTING) != 0
              ? "set rasterizer_soft_filter true"
              : "set rasterizer_soft_filter false");

            output.AppendLine("\n;;;  If TRUE, disable Lens Dirt");
            output.AppendLine((Shaders & PP.LENS_DIRT) != 0
              ? "set use_super_remote_players_action_update false"
              : "set use_super_remote_players_action_update true");

            output.AppendLine("\n;;;  If TRUE, disable Film Grain");
            output.AppendLine((Shaders & PP.FILM_GRAIN) != 0
              ? "set use_new_vehicle_update_scheme false"
              : "set use_new_vehicle_update_scheme true");

            output.AppendLine("\n;;;  If TRUE, disable immersive helmet/visor overlay");
            output.AppendLine((Shaders & PP.HUD_VISOR) != 0
              ? "set multiplayer_draw_teammates_names false"
              : "set multiplayer_draw_teammates_names true");

            output.AppendLine("\n;;;  Toggle Screen-Space, path-traced Reflections");
            output.AppendLine((Shaders & PP.SSR) != 0
              ? "set error_suppress_all true"
              : "set error_suppress_all false");

            output.AppendLine("\n;;;  If TRUE, disable Adaptive HDR");
            output.AppendLine((Shaders & PP.ADAPTIVE_HDR) != 0
              ? "set director_camera_switch_fast 0"
              : "set director_camera_switch_fast 1");

            if (System.IO.File.Exists(Paths.Legacy))
            {
                output.AppendLine("\n;;;  Toggle Dynamic Lens Flares");
                output.AppendLine((Shaders & PP.DYNAMIC_LENS_FLARES) != 0
                  ? "set display_precache_progress true"
                  : "set display_precache_progress false");
            }

            if (!System.IO.File.Exists(Paths.Legacy))
            {
                output.AppendLine("\n;;;  Toggle Color Deband");
                output.AppendLine((Shaders & PP.DEBAND) != 0
                  ? "set display_precache_progress true"
                  : "set display_precache_progress false");
            }

            /* motion blur */
            output.AppendLine("\n;;;  Set Motion Blur: {1.0, 1.1, 1.2, 1.3}; 1.0 == off");
            output.AppendLine("set multiplayer_hit_sound_volume " + new Func<string>(() =>
            {
                if ((Shaders & PP.MOTION_BLUR_POMB_HIGH) != 0)
                    return "1.3";

                if ((Shaders & PP.MOTION_BLUR_POMB_LOW) != 0)
                    return "1.2";

                if ((Shaders & PP.MOTION_BLUR_BUILT_IN) != 0)
                    return "1.1";

                return "1.0";
            })());

            /* mxao */
            output.AppendLine("\n;;;  Set MartyMcFly's MX Ambient Occlusion: {2, 3, 4}; 2 == off");
            output.AppendLine("set cl_remote_player_action_queue_limit " + new Func<string>(() =>
            {
                if ((Shaders & PP.MXAO_HIGH) != 0)
                    return "4";

                if ((Shaders & PP.MXAO_LOW) != 0)
                    return "3";

                return "2";
            })());

            /* depth of field */
            output.AppendLine("\n;;;  Set Depth of Field {6, 7, 8}; 6 == off");
            output.AppendLine("set cl_remote_player_action_queue_tick_limit " + new Func<string>(() =>
            {
                if ((Shaders & PP.DOF_HIGH) != 0)
                    return "8";

                if ((Shaders & PP.DOF_LOW) != 0)
                    return "7";

                return "6";
            })());

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