using System;
using System.IO;
using Microsoft.Win32;

namespace HXE.HCE
{
  public class Registry
  {
    private static string _dpid   = BogusDPID;
    
    public const string x86       = @"SOFTWARE\Microsoft";
    public const string x86_64    = @"SOFTWARE\Wow6432Node\Microsoft";
    public const string MSG       = "Microsoft Games";
    public const string Retail    = "Halo";
    public const string Custom    = "Halo CE";
    public const string Trial     = "Halo Trial";
    public const string HEK       = "Halo HEK";
    public const string BogusPID  = "00000-000-0000000-00000";
    public const string BogusDPID = "a40000000300000030303030302d3030302d303030303030302d303030303000500000004d30302d30303030300000000000000046203249cdb922e66221b02cc3ee01000000000082610f5b913a8a030000000000000000000000000000000000000000000000003030303030000000000000000b0d0000ba6d6b82000800000000000000000000000000000000000000000000000000000000000000000000f9a404e6";
    public static string PID      = BogusPID;
    public static byte[] ByteDPID = StringToByteArray(PID);
    /** TODO:
     * > Reverse-engineer Product Activation Key system.
     * > Allow the user to input their legitimate key.
     * > Reverse-engineer PID/DPID.
     *    They're related to the Product Key
     *    and Halo Retail/Custom requires these
     *    two registry values to run. Else, it
     *    complains that it isn't activated/legitimate.
     *  > If HXE is used for installing/deploying a
     *    Halo package, assign the Target path to
     *    data.EXE_Path.
     *    Additionally, assign the language enum as 
     *    indicated by the package to data.LangID.
     */

    public static string StringDPID
    {
      get => _dpid;
      set
      {
        if (value == _dpid) return;
        value = value.Replace(" ", "");
        ByteDPID = StringToByteArray(value);
        _dpid = value;
      }
    }

    public static byte[] StringToByteArray(string hex)
    {
      int    sLength = hex.Length;
      byte[] bytes   = new byte[sLength / 2];
      for (int i = 0; i < sLength; i+=2)
      {
        bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
      }
      return bytes;
    }

    public static bool KeyExists(string keyPath)
    {
      var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(keyPath);
      return key != null;
    }

    public static string WoWCheck()
    {
      return KeyExists(x86_64) ? x86_64 : x86;
    }

    public static void CreateKeys(string game, string path)
    {
      var key  = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(path);
      var data = new Data();
      // https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registry.setvalue?view=netframework-4.6#Microsoft_Win32_Registry_SetValue_System_String_System_String_System_Object_Microsoft_Win32_RegistryValueKind_
      // https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registryvaluekind?view=netframework-4.6

      /// Create the game's registry key if it doesn't exist.
      if (key == null)
      {
        var key2 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(Path.Combine(WoWCheck(), MSG));
        if (key2 == null)
        {
          var key3 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(WoWCheck());
          key3.CreateSubKey(MSG);
        }
        key2.CreateSubKey(game);
      }

      switch (game)
      {
        case "Retail":
          { 
            data.Version     = "1.10";
            data.VersionType = "RetailVersion";
            key.SetValue("CDPath"          , data.CDPath          , RegistryValueKind.String);
            key.SetValue("DigitalProductID", data.DigitalProductID, RegistryValueKind.Binary);
            key.SetValue("DistID"          , data.DistID          , RegistryValueKind.DWord);
            key.SetValue("EXE Path"        , data.EXE_Path        , RegistryValueKind.String);
            key.SetValue("InstalledGroup"  , data.InstalledGroup  , RegistryValueKind.String);
            key.SetValue("LangID"          , data.LangID          , RegistryValueKind.DWord);
            key.SetValue("Launched"        , data.Launched        , RegistryValueKind.String);
            key.SetValue("PendingVersion"  , data.PendingVersion  , RegistryValueKind.String);
            key.SetValue("PID"             , data.PID             , RegistryValueKind.String);
            key.SetValue("Version"         , data.Version         , RegistryValueKind.String);
            key.SetValue("VersionType"     , data.VersionType     , RegistryValueKind.String);
            key.SetValue("Zone"            , data.Zone            , RegistryValueKind.String);
          }
          break;

        case "Custom":
          { 
            data.Version     = "1.10";
            data.VersionType = "TrialVersion";
            key.SetValue("CDPath"          , data.CDPath          , RegistryValueKind.String);
            key.SetValue("DigitalProductID", data.DigitalProductID, RegistryValueKind.Binary);
            key.SetValue("DistID"          , data.DistID          , RegistryValueKind.DWord);
            key.SetValue("EXE Path"        , data.EXE_Path        , RegistryValueKind.String); /// EXE_Path = SPV3.Installer.Target
            key.SetValue("InstalledGroup"  , data.InstalledGroup  , RegistryValueKind.String);
            key.SetValue("LangID"          , data.LangID          , RegistryValueKind.DWord);
            key.SetValue("Launched"        , data.Launched        , RegistryValueKind.String);
            key.SetValue("PendingVersion"  , data.PendingVersion  , RegistryValueKind.String);
            key.SetValue("PID"             , data.PID             , RegistryValueKind.String);
            key.SetValue("Version"         , data.Version         , RegistryValueKind.String);
            key.SetValue("VersionType"     , data.VersionType     , RegistryValueKind.String);
          }
          break;

        case "Trial":
          { 
            data.DigitalProductID = null;
            data.PID              = null;
            data.Version          = "1";
            data.VersionType = "TrialVersion";
            key.SetValue("CDPath"          , data.CDPath          , RegistryValueKind.String);
            key.SetValue("DigitalProductID", data.DigitalProductID, RegistryValueKind.Binary);
            key.SetValue("EXE Path"        , data.EXE_Path        , RegistryValueKind.String);
            key.SetValue("InstalledGroup"  , data.InstalledGroup  , RegistryValueKind.String);
            key.SetValue("LangID"          , data.LangID          , RegistryValueKind.DWord);
            key.SetValue("Launched"        , data.Launched        , RegistryValueKind.String);
            key.SetValue("PID"             , data.PID             , RegistryValueKind.String);
            key.SetValue("Version"         , data.Version         , RegistryValueKind.String);
            key.SetValue("VersionType"     , data.VersionType     , RegistryValueKind.String);
          }
          break;

        case "HEK":
          { 
            data.DigitalProductID = null;
            data.PID              = null;
            data.VersionType      = "TrialVersion";
            key.SetValue("CDPath"          , data.CDPath          , RegistryValueKind.String);
            key.SetValue("DigitalProductID", data.DigitalProductID, RegistryValueKind.Binary);
            key.SetValue("EXE Path"        , data.EXE_Path        , RegistryValueKind.String);
            key.SetValue("InstalledGroup"  , data.InstalledGroup  , RegistryValueKind.String);
            key.SetValue("LangID"          , data.LangID          , RegistryValueKind.DWord);
            key.SetValue("Launched"        , data.Launched        , RegistryValueKind.String);
            key.SetValue("PID"             , data.PID             , RegistryValueKind.String);
            key.SetValue("VersionType"     , data.VersionType     , RegistryValueKind.String);
          }
          break;

        default: 
          break;
      }
    }

    /// <summary>
    /// Read Windows Registry entries for the selected game
    /// or create them if they don't exist.
    /// </summary>
    /// <param name="game">
    /// Pass Custom to look for the Custom Edition registry entries
    /// or Retail to look for Retail registry entries.
    /// </param>
    public static void GetRegistryKeys(string game)
    {
      var path = Path.Combine(WoWCheck(), MSG);
      switch (game)
      {
        case "Retail":
          path = Path.Combine(path, Retail);
          if (!KeyExists(path))
            CreateKeys(game, path);
          // read to memory
          break;
        case "Custom":
          path = Path.Combine(path, Custom);
          if (!KeyExists(path))
            CreateKeys(game, path);
          break;
        case "Trial":
          path = Path.Combine(path, Trial);
          if (!KeyExists(path))
            CreateKeys(game, path);
          break;
        case "HEK":
          path = Path.Combine(path, HEK);
          if (!KeyExists(path))
            CreateKeys(game, path);
          break;
        default:
          break;
      }
    }
    public class Data
    {
      /** ---- Halo 1 Registry Keys ----
       *  Retail : Halo
       *  Custom : Halo CE
       *  Trial  : Halo Trial
       *  HEK    : Halo HEK
       */

      /** ---- SubKey Values and Rules ---- */
      /// <summary>
      /// Applies to: All
      /// </summary>
      public string          CDPath           = Environment.CurrentDirectory;
      /// <summary>
      /// Applies to: All
      /// </summary>
      /// <remarks>Only Retail, Custom fill value</remarks>
      public byte[]          DigitalProductID = ByteDPID;
      /// <summary>
      /// Applies to: Retail, Custom
      /// </summary>
      public readonly int    DistID           = 860; // 0x35c
      ///<summary>
      /// Applies to: All
      /// </summary>
      public string          EXE_Path         = "";
      /// <summary>
      /// Applies to: All
      /// </summary>
      public readonly string InstalledGroup   = "1";
      /// <summary>
      /// Applies to: All;
      /// </summary>
      /// <remarks>9 == English</remarks>
      public byte            LangID           = 9;
      /// <summary>
      /// Applies to: All
      /// </summary>
      public readonly string Launched         = "1";
      /// <summary>
      /// Applies to: Retail, Custom
      /// </summary>
      /// <remarks>Typically no value. Probably only used by HaloUpdate.exe</remarks>
      public string          PendingVersion   = "";
      /// <summary>
      /// Applies to: All
      /// </summary>
      /// <remarks>Only Custom, Retail fill value</remarks>
      public string          PID              = Registry.PID;
      /// <summary>
      /// Applies to: Retail, Custom, Trial
      /// </summary>
      /// <remarks>Trial is "1". Retail, Custom can be 1.10 or older.</remarks>
      public string          Version          = "";
      /// <summary>
      /// Applies to: All
      /// </summary>
      /// <remarks>If Retail, RetailVersion. Else, TrialVersion.</remarks>
      public string          VersionType      = "";
      /// <summary>
      /// Applies to: Retail
      /// </summary>
      public readonly string Zone             = "http://www.zone.com/asp/script/default.asp?Game=Halo&password=Password";

      /// SubKey Examples
      /** Halo 
       *  | Type   | Name             | Value
       *  | ------ | ---------------- | -----
       *  | String | CDPath           | C:\Users\Noah\Desktop\Halo PC\
       *  | Binary | DigitalProductID | a40000000300000030303030302d3030302d303030303030302d303030303000500000004d30302d30303030300000000000000046203249cdb922e66221b02cc3ee01000000000082610f5b913a8a030000000000000000000000000000000000000000000000003030303030000000000000000b0d0000ba6d6b82000800000000000000000000000000000000000000000000000000000000000000000000f9a404e6
       *  | DWord  | DistID           | 35c
       *  | String | EXE Path         | J:\Games\Halo Retail
       *  | String | InstalledGroup   | 1
       *  | DWord  | LangID           | 9
       *  | String | Launched         | 1
       *  | String | PendingVersion   | 
       *  | String | PID              | 75043-035-5925194-40507
       *  | String | Version          | 1.10
       *  | String | VersionType      | RetailVersion
       *  | String | Zone             | http://www.zone.com/asp/script/default.asp?Game=Halo&password=Password
       */

      /** Halo CE
       *  | Type   | Name             | Value
       *  | ------ | ---------------- | -----
       *  | String | CDPath           | H:\Downloads\Games\Halo1\Installers and Official Files\
       *  | Binary | DigitalProductID | a40000000300000037353034332d3033352d353932353139342d343030363000500000004d36312d30303033320000000000000046203249cdb922e66221b02cc3ee01000000000082610f5b913a8a030000000000000000000000000000000000000000000000003037353030000000000000000b0d0000ba6d6b82000800000000000000000000000000000000000000000000000000000000000000000000f9a404e6
       *  | DWord  | DistID           | 35c
       *  | String | EXE Path         | J:\Games\Halo Custom Edition
       *  | String | InstalledGroup   | 1
       *  | DWord  | LangID           | 9
       *  | String | Launched         | 1
       *  | String | PendingVersion   | 
       *  | String | PID              | 75043-035-5925194-40060
       *  | String | Version          | 1.10
       *  | String | VersionType      | TrialVersion
       */

      /** Halo Trial
       *  | Type   | Name             | Value
       *  | ------ | ---------------- | -----
       *  | String | CDPath           | H:\Downloads\Games\Halo1\
       *  | Binary | DigitalProductID | 
       *  | String | EXE Path         | J:\Games\Halo Trial
       *  | String | InstalledGroup   | 1
       *  | DWord  | LangID           | 9
       *  | String | Launched         | 1
       *  | String | PID              | 
       *  | String | Version          | 1.10
       *  | String | VersionType      | TrialVersion
       */

      /** Halo HEK
       *  | Type   | Name             | Value
       *  | ------ | ---------------- | -----
       *  | String | CDPath           | H:\Downloads\
       *  | Binary | DigitalProductID | 
       *  | String | EXE Path         | J:\Games\Halo Custom Edition
       *  | String | InstalledGroup   | 1
       *  | DWord  | LangID           | 9
       *  | String | Launched         | 1
       *  | String | PID              | 
       *  | String | VersionType      | TrialVersion
       */
    }
  }
}
