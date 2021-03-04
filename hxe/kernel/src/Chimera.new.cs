using System;
using FVI = System.Diagnostics.FileVersionInfo;


namespace HXE
{
  public class ChimeraModule
  {
    public static bool   Found    = FVI.ProductName == "Chimera";
    public static string Module   = System.IO.Path.Combine(Environment.CurrentDirectory, "strings.dll");
    public static FVI    FVI      = FVI.GetVersionInfo(Module);
    public static string PVersion = FVI.ProductVersion;
    
    public void Detect()
    {
      Found = FVI.ProductName == "Chimera";
    }
  }

  /// <summary>
  /// Object-represenation of chimera.ini
  /// </summary>
  /// <remarks>
  /// Up-to-date with https://github.com/SnowyMouse/chimera/blob/7bb372f93c076124a4cfeaff575cb8b0ed24cd37/chimera.ini
  /// </remarks>
  public class ChimeraIni
  {
    /// <summary>
    ///   Halo initialization settings
    /// </summary>
    /// <remark>
    ///   This is used to configure how Halo is initialized.
    /// </remark>
    struct halo
    {
      /** Set this to change where profiles are stored.
       * It can be absolute or relative and can be overridden with -path.
       */
      string path;

      /** Set this to change where downloaded maps are stored.
       * It can be absolute or relative.
       * By default it is set to <path>\chimera\maps.
       */
      string download_map_path;

      /** Set this to change where to load maps from.
       * It can be absolute or relative.
       * By default, it is set to ./maps.
       */
      string map_path;

      /** Set this to execute a given list of commands in an 8-bit text file. */
      string exec;

      /** Set ports
       * this can be overridden with -port and -cport, respectively.
       * You can set client_port=0 to have the OS assign a client port,
       * which is recommended if you run multiple Halo instances on your PC.
       */
      int server_port;
      int client_port;

      /** Enable the intro videos
       * This is normally disabled by default as it makes
       * starting up the game take much longer and is annoying.
       * Turning this on will also turn on the demo outro video
       * *if* on the demo version of the game.
       */
      bool intro_videos;

      /** Set this to 1 to use the console
       * this can be overridden by -console
       */
      bool console;

      /** Set this to 1 to use optimal defaults.
       * This is intended to makes the game play more like
       * a proper PC game, enabling the following settings:
       *   chimera_af true
       *   chimera_aim_assist true
       *   chimera_block_loading_screen true
       *   chimera_block_mouse_acceleration true
       *   chimera_diagonals 0.75
       *   chimera_fov auto
       *   chimera_fov_cinematic auto
       *   chimera_fp_reverb true
       *   chimera_throttle_fps 300
       *   chimera_uncap_cinematic true
       */
      bool optimal_defaults;

      /** Set this to 0 to disable the menu music */
      bool main_menu_music;

      /** Set this to 1 to enable video when Halo is not in focus (e.g. tabbed out) */
      bool background_playback;

      /** Set this to 1 to enable multiple instances
       *
       * NOTE: Each instance needs its own client port to join games. Setting
       * client_port=0 CAN ensure this.
       *
       * NOTE: Due to how Halo handles save files, enabling this will result in the
       * game crashing if you create checkpoints. If you intend to play the campaign,
       * turn this off!
       */
      bool multiple_instances;

      /** Use a custom hash
       * string up to 32 characters OR "%" for a random hash
       */
      string hash;
    }

    /// <summary>
    ///   Error handling settings
    /// </summary>
    /// <remarks>
    ///   This is used to configure error handling
    /// </remarks>
    struct error_handling
    {
      /** Set this to 1 to not show any error boxes created by Chimera */
      bool suppress_fatal_errors;

      /** Set this to 1 to show segmentation fault errors */
      bool show_segmentation_fault;
    }

    /// <summary>
    ///   Video mode settings
    /// </summary>
    /// <remarks>
    ///   This is used to configure video mode settings
    /// </remarks>
    struct video_mode
    {
      /** Enable the [video_mode] settings */
      bool enabled;

      /** Resolution to use
       *
       * Putting auto will use your primary monitor's current width or height.
       *
       * NOTE: Halo may fail to start properly if the width or height is not supported
       * by your desktop.
       */
      string width;  // uint, auto
      string height; // uint, auto

      /** Refresh rate (Hz)
       *
       * Putting 0 here will use your system's current refresh rate.
       *
       * NOTE: While the Halo menu does not list refresh rates above 120 Hz, this
       * setting can be used to bypass this limitation.
       */
      uint refresh_rate;

      /** Enable double buffer vSync?
       *
       * This can be used to prevent tearing, but it comes at the cost of several
       * frames of input lag (often times >100 ms if @ 60 Hz).
       *
       * NOTE: If you have access to a monitor with a variable refresh rate (e.g.
       * FreeSync, G-Sync, etc.), then it is HIGHLY RECOMMENDED to, instead, use
       * chimera_throttle_fps to lock your frame rate to your refresh rate minus 3 (so,
       * if you're on a 144 Hz display, lock it to 141 FPS) and play in full screen,
       * rather than enabling vSync.
       */
      bool vsync;

      /** Play in a window?
       * 
       * On newer versions of Windows, this will incur some input lag due to the
       * Desktop Window Manager. If you're on Linux, it is recommended to disable
       * compositing if you choose to play in a window.
       */
      bool windowed;

      /** Play in borderless fullscreen?
       * 
       * You will need to be in windowed mode (-window or windowed=1) for this to work.
       * This can be used to effectively play in full screen without changing video
       * modes, although you may experience higher input lag than exclusive full screen
       * mode (see video_mode.windowed remark).
       *
       * Note that resolutions lower than your current resolution will be stretched.
       */
      bool borderless;
    }

    /// <summary>
    ///   Scoreboard settings
    /// </summary>
    /// <remarks>
    ///   This is used to customize the scoreboard.
    /// </remarks>
    struct scoreboard
    {
      /** Set the font
       * can be smaller, small, large, console, system
       */
      string font;

      /** Set the fade time in seconds
       * 0.5 = default, 0 = instant
       */
      float  fade_time;
    }

    /// <summary>
    ///   Name settings
    /// </summary>
    /// <remarks>
    ///   This is used to customize names displayed when looking at players.
    /// </remarks>
    struct name 
    {
      /** Set the font
       * can be smaller, small, large, console, system
       */
      string font;
    }

    /// <summary>
    ///   Server list settings
    /// </summary>
    /// <remarks>
    ///   This is used to configure how the server list works.
    /// </remarks>
    struct server_list
    {
      /** Set this to 1 to enable automatically getting the server list on open. */
      bool auto_query;
    }

    /// <summary>
    ///   Map memory settings
    /// </summary>
    /// <remarks>
    ///   This is used to configure how to handle how maps are loaded by Chimera.
    /// </remarks>
    struct memory
    {
      /** Enable this to load maps directly into RAM rather than use temporary files. */
      bool enable_map_memory_buffers;

      /** Size of buffer (in MiB) to allocate for both the ui and one non-ui map */
      uint map_size;

      /** Show the time it took to decompress maps */
      bool benchmark;

      /** Font to use when downloading
       * can be smaller, small, large, console, system
       */
      string download_font;

      /** Preferred download server number 
       * by default, it's 1, then 2 if 1 fails, then 3 if 2 fails, etc.
       * Choosing a different server may yield better speeds.
       */
      byte download_preferred_node;

      /** Enable downloading of retail (AKA HaloMD / Halo PC) maps. 
       * If this is disabled, then you will get an error if you try to download such maps.
       * This does NOT prevent Custom Edition or trial maps from auto-downloading.
       * 
       * NOTE: This is OFF by default due to potentially downloading incompatible maps.
       * Ensure you have an English copy of retail Halo PC bitmaps.map and sounds.map
       * before turning on this option. Incompatible maps will result in corrupt assets
       * loading which can result in extremely loud sounds (aka "earrape").
       */
      bool download_retail_maps;
    }

    /// <summary>
    ///   Font Override settings
    /// </summary>
    /// <remarks>
    ///   Override the fonts used by Chimera with system fonts. <br/>
    ///   This may have issues on Wine. <br/>
    ///   Enabling this will also force the widescreen fix to be on. <br/>
    ///   You will need to have the latest Direct3D 9 runtime installed. <br/>
    ///   To install it, go here: https://www.microsoft.com/en-us/download/details.aspx?id=35 
    /// </remarks>
    struct font_override
    {
      /** Enable overriding Chimera's fonts */
      bool enabled;


      /** System font override */
      bool system_font_override;
      
      /** Font family */
      string system_font_family;

      /** Font weight (system)
       * Scale of 100 (light) to 900 (boldest) with 400 being normal;
       * not all weights work on all fonts.
       */
      short system_font_weight;
      /** Font size */
      byte system_font_size;
      /** X/Y offsets */
      short system_font_x_offset;
      short system_font_y_offset;

      /** Shadow offset
       * if all 0, don't use shadows - much faster but harder to read
       */
      short system_font_shadow_offset_x;
      short system_font_shadow_offset_y;


      /** Large font override */
      bool large_font_override;

      /** Font family */
      string large_font_family;

      /** Font weight (large); 
       * Scale of 100 (light) to 900 (boldest) with 400 being normal;
       * not all weights work on all fonts.
       */
      short  large_font_weight;

      /** Font size */
      byte   large_font_size;

      /** X/Y offsets */
      short  large_font_offset_x;
      short  large_font_offset_y;

      /** Shadow offset
       * if all 0, don't use shadows - much faster but harder to read
       */
      short  large_font_shadow_offset_x;
      short  large_font_shadow_offset_y;


      /** Small font override */
      bool   small_font_override;

      /** Font family */
      string small_font_family;

      /** Font weight (small);
       * Scale of 100 (light) to 900 (boldest) with 400 being normal;
       * not all weights work on all fonts.
       */
      short  small_font_weight;

      /** Font size */
      byte   small_font_size;

      /** X/Y offsets */
      short small_font_offset_x;
      short small_font_offset_y;

      /** Shadow offset
       * if all 0, don't use shadows - much faster but harder to read
       */
      short small_font_shadow_offset_x;
      short small_font_shadow_offset_y;

      /** Smaller font override */
      bool smaller_font_override;

      /** Font family */
      string smaller_font_family;

      /** Font weight (smaller);
       * Scale of 100 (light) to 900 (boldest) with 400 being normal;
       * not all weights work on all fonts.
       */
      short smaller_font_weight;

      /** Font size */
      byte smaller_font_size;

      /** X/Y offsets */
      short smaller_font_offset_x;
      short smaller_font_offset_y;

      /** Shadow offset
       * if all 0, don't use shadows - much faster but harder to read
       */
      short smaller_font_shadow_offset_x;
      short smaller_font_shadow_offset_y;

      /** Ticker font override */
      bool ticker_font_override;

      /** Font family */
      string ticker_font_family;

      /** Font weight (ticker);
       * Scale of 100 (light) to 900 (boldest) with 400 being normal;
       * not all weights work on all fonts.
       */
      short ticker_font_weight;

      /** Font size */
      byte ticker_font_size;

      /** X/Y offsets */
      short ticker_font_offset_x;
      short ticker_font_offset_y;

      /** Shadow offset
       * if all 0, don't use shadows - much faster but harder to read
       */
      short ticker_font_shadow_offset_x;
      short ticker_font_shadow_offset_y;


      /** Console font override */
      bool console_font_override;

      /** Font family */
      string console_font_family;
      
      /** Font weight (console);
       * Scale of 100 (light) to 900 (boldest) with 400 being normal;
       * not all weights work on all fonts.
       */
      short console_font_weight;

      /** Font size (console) */
      byte  console_font_size;

      /** X/Y offsets */
      short console_font_offset_x;
      short console_font_offset_y;

      /** Shadow offset
       * if all 0, don't use shadows - much faster but harder to read
       */
      short console_font_shadow_offset_x;
      short console_font_shadow_offset_y;
    }

    /// <summary>
    ///   Controller settings
    /// </summary>
    /// <remarks>
    ///   This is used to configure gamepads.
    /// </remarks>
    struct controller
    {
      /**
       *  Enable this to use this feature
       * enabled=1
       * 
       *  You can put button text here. Examples:
       *      button_1=Button 1
       *      axis_2_n=Axis 2 (-)
       *      axis_3_p=Axis 3 (+)
       *      pov_4_e=POV 4 (East)
       * 
       *  Alternatively, you can find premade controller configs here:
       *      https://github.com/SnowyMouse/chimera/tree/master/controller_config
       */

      bool enabled;

      string axis_1_n;
      string axis_1_p;

      string axis_2_n;
      string axis_2_p;

      string axis_3_n;
      string axis_3_p;
      
      string axis_4_n;
      string axis_4_p;

      string axis_5_n;
      string axis_5_p;

      string axis_6_n;
      string axis_6_p;

      string button_1;
      string button_2;
      string button_3;
      string button_4;
      string button_5;
      string button_6;
      string button_7;
      string button_8;
      string button_9;
      string button_10;
      string button_11;
      string button_12;
      string button_13;
      string button_14;
      string button_15;
      string button_16;
      string button_17;
      string button_18;
      string button_19;
      string button_20;
      string button_21;
      string button_22;
      string button_23;
      string button_24;

      string pov_1_w;
      string pov_1_e;
      string pov_1_n;
      string pov_1_s;
    }

    /// <summary>
    ///   Custom console configuration
    /// </summary>
    /// <remarks>
    ///   This is the custom console settings.
    /// </remarks>
    struct custom_console
    {
      /** Enable custom console */
      bool enabled;

      /** Enable scrollback?
       * page up/down
       */
      bool enable_scrollback;

      /** Maximum lines */
      uint buffer_size;

      /** Soft limit
       * delete old lines after this many lines are hit
       */
      uint buffer_size_soft;

      /** Line height
       * 1.0 = potentially no gap between lines
       */
      float line_height;

      /** Margins on left and right side */
      short x_margin;

      /** Time in seconds before lines start fading */
      float fade_start;

      /** Time in seconds for lines to completely fade */
      float fade_time;
    }

    /// <summary>
    ///   Custom chat configuration
    /// </summary>
    /// <remarks>
    ///   This is the custom chat and server chat. <br/>
    ///   Fonts can be either system, console, small, smaller, or large, and will be map-specific.
    /// </remarks>
    struct custom_chat
    {
      /** Server message color
       * alpha, red, green, blue;
       * 0-1 intensity
       */
      float server_message_color_a;
      float server_message_color_r;
      float server_message_color_g;
      float server_message_color_b;

      /** Server message offset
       * x and y;
       * HUD pixels
       */
      int server_message_x;
      int server_message_y;

      /** Server message dimensions
       * width and height;
       * HUD pixels
       */
      int server_message_w;
      int server_message_h;

      /** Server message height when chat is open
       * HUD pixels
       */
      int server_message_h_chat_open;

      /** Server message anchor
       * can be top_left, top_right, center, bottom_left, bottom_right 
       */
      string server_message_anchor;

      /** Server message hide when console is visible */
      bool server_message_hide_on_console;

      /** Server message font
       * can be smaller, small, large, console, system
       */
      string server_message_font;

      /** Server message animation time in seconds */
      uint server_slide_time_length;

      /** Server message display time in seconds (when chat is not in focus) */
      float server_time_up;

      /** Server message fade out time in seconds */
      float server_fade_out_time;

      /** Server line height */
      float server_line_height;


      /** Chat message color - Free-for-all
       * alpha, red, green, blue;
       * 0-1 intensity
       */
      float chat_message_color_ffa_a;
      float chat_message_color_ffa_r;
      float chat_message_color_ffa_g;
      float chat_message_color_ffa_b;

      /** Chat message color - Red team
       * alpha, red, green, blue;
       * 0-1 intensity
       */
      float chat_message_color_red_a;
      float chat_message_color_red_r;
      float chat_message_color_red_g;
      float chat_message_color_red_b;

      /** Chat message color - Blue team
       * alpha, red, green, blue;
       * 0-1 intensity
       */
      float chat_message_color_blue_a;
      float chat_message_color_blue_r;
      float chat_message_color_blue_g;
      float chat_message_color_blue_b;

      /** Chat message offset
       * x and y;
       * HUD pixels
       */
      uint chat_message_x;
      uint chat_message_y;

      /** Chat message height when chat is open
       * HUD pixels
       */
      uint chat_message_w;
      uint chat_message_h;

      /** Chat message height when chat is open
       * HUD pixels
       */
      uint chat_message_h_chat_open;

      /** Chat message anchor
       * can be top_left, top_right, center, bottom_left, bottom_right
       */
      string chat_message_anchor;

      /// Chat message hide when console is visible
      bool chat_message_hide_on_console;

      /** Chat message font
       * can be smaller, small, large, console, system */
      string chat_message_font;

      /// Chat message animation time in seconds
      float chat_slide_time_length;

      /** Chat message display time in seconds (when chat is not in focus) */
      float chat_time_up;

      /** Chat message fade out time in seconds */
      float chat_fade_out_time;

      /** Chat line height */
      float chat_line_height;

      /** Chat input color
       * alpha, red, green, blue;
       * 0-1 intensity
       */
      float chat_input_color_a;
      float chat_input_color_r;
      float chat_input_color_g;
      float chat_input_color_b;

      /** Chat input offset
       * x and y;
       * HUD pixels
       */
      uint chat_input_x;
      uint chat_input_y;

      /** Chat input width
       * HUD pixels
       */
      uint chat_input_w;

      /** Chat input anchor
       * can be top_left, top_right, center, bottom_left, bottom_right
       */
      string chat_input_anchor;

      /** Chat input font
       * can be smaller, small, large, console, system
       */
      string chat_input_font;
    }

    /// <summary>
    ///   Hotkey configuration
    /// </summary>
    /// <remarks>
    ///   Hotkeys can be configured to emit EITHER Halo scripts <i>OR</i> Chimera commands.
    /// </remarks>
    struct hotkey
    {
      /** Enable this to use hotkeys */
      bool enabled;

      /** Function key hotkeys
       * f5 (send "taxi" to chat - useful for servers that have this script) */
      string f1;
      string f2;
      string f3;
      string f4;
      string f5;
      string f6;
      string f7;
      string f8;
      string f9;
      string f10;
      string f11;
      string f12;

      /** Alt+Shift+# hotkeys
       * alt-shift-1 through alt-shift-3: chimera_spectate
       * alt-shift-4: Toggle the debug camera 
       */
      string alt_shift_1;
      string alt_shift_2;
      string alt_shift_3;
      string alt_shift_4;
      string alt_shift_5;
      string alt_shift_6;
      string alt_shift_7;
      string alt_shift_8;
      string alt_shift_9;
      string alt_shift_0;
      /// Numpad
      string alt_shift_num_1;
      string alt_shift_num_2;
      string alt_shift_num_3;
      string alt_shift_num_4;
      string alt_shift_num_5;
      string alt_shift_num_6;
      string alt_shift_num_7;
      string alt_shift_num_8;
      string alt_shift_num_9;
      string alt_shift_num_0;

      /** Alt+# hotkeys (default: bookmarks)
       * Defaults:
       *   alt-1 through alt-9: connect to bookmark
       *   alt-0: list bookmarks 
       */
      string alt_1;
      string alt_2;
      string alt_3;
      string alt_4;
      string alt_5;
      string alt_6;
      string alt_7;
      string alt_8;
      string alt_9;
      string alt_0;
      /// Numpad
      string alt_num_1;
      string alt_num_2;
      string alt_num_3;
      string alt_num_4;
      string alt_num_5;
      string alt_num_6;
      string alt_num_7;
      string alt_num_8;
      string alt_num_9;
      string alt_num_0;

      /** Ctrl+# hotkeys (default: history)
       * Defaults:
       *   ctrl-1 through ctrl-9: connect to history
       *   ctrl-0: list history
       */
      string ctrl_1;
      string ctrl_2;
      string ctrl_3;
      string ctrl_4;
      string ctrl_5;
      string ctrl_6;
      string ctrl_7;
      string ctrl_8;
      string ctrl_9;
      string ctrl_0;
      /// Numpad
      string ctrl_num_1;
      string ctrl_num_2;
      string ctrl_num_3;
      string ctrl_num_4;
      string ctrl_num_5;
      string ctrl_num_6;
      string ctrl_num_7;
      string ctrl_num_8;
      string ctrl_num_9;
      string ctrl_num_0;

      /** Ctrl+Alt+Shift+# hotkeys
       * Defaults:
       *   ctrl-alt-shift-1: block currently held weapon(if in a game where you hold 3+ weapons)
       *   ctrl-alt-shift-2: unblock currently held weapon(if in a game where you hold 3+ weapons)
       *   ctrl-alt-shift-3: abort the current cutscene(useful for maps that have annoying cutscenes when you start) 
       */
      string ctrl_alt_shift_1;
      string ctrl_alt_shift_2;
      string ctrl_alt_shift_3;
      string ctrl_alt_shift_4;
      string ctrl_alt_shift_5;
      string ctrl_alt_shift_6;
      string ctrl_alt_shift_7;
      string ctrl_alt_shift_8;
      string ctrl_alt_shift_9;
      string ctrl_alt_shift_0;
      /// Numpad
      string ctrl_alt_shift_num_1;
      string ctrl_alt_shift_num_2;
      string ctrl_alt_shift_num_3;
      string ctrl_alt_shift_num_4;
      string ctrl_alt_shift_num_5;
      string ctrl_alt_shift_num_6;
      string ctrl_alt_shift_num_7;
      string ctrl_alt_shift_num_8;
      string ctrl_alt_shift_num_9;
      string ctrl_alt_shift_num_0;

      /// alt_f4=chimera_aimbot 1
    }
  }
}
