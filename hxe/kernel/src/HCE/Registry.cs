using System;
using System.IO;

namespace HXE.HCE
{
  class Registry
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
     */

    public static string DPID
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
      var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(path);

      switch (game)
      {
        case "Retail":
          if (key == null)
          {
            var key2 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(Path.Combine(WoWCheck(), MSG));
            key2.CreateSubKey(Retail);
          }
          // https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registry.setvalue?view=netframework-4.6#Microsoft_Win32_Registry_SetValue_System_String_System_String_System_Object_Microsoft_Win32_RegistryValueKind_
          // https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registryvaluekind?view=netframework-4.6
          key.SetValue("CDPath", "", (Microsoft.Win32.RegistryValueKind) 1);
          break;
        case "Custom":
          if (key == null)
          {
            var key2 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(Path.Combine(WoWCheck(), MSG));
            key2.CreateSubKey(Custom);
          }
          break;
        case "Trial":
          if (key == null)
          {
            var key2 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(Path.Combine(WoWCheck(), MSG));
            key2.CreateSubKey(Trial);
          }
          break;
        case "HEK":
          if (key == null)
          {
            var key2 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(Path.Combine(WoWCheck(), MSG));
            key2.CreateSubKey(HEK);
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

          break;
        case "Custom":
          path = Path.Combine(path, Custom);
          if (!KeyExists(path))
            CreateKeys(game, path);
          break;
        case "Trial":
          break;
        case "HEK":
          break;
        default:
          break;
      }
    }
    class Data
    {
      /** Halo 1 Registry Keys
       *  Retail : Halo
       *  Custom : Halo CE
       *  Trial  : Halo Trial
       *  HEK    : Halo HEK
       */

      /** Key Values */
      
      /// <summary>
      /// Applies to: All;
      /// </summary>
      string          CDPath         = "";
      /// <summary>
      /// Applies to: All;
      /// Only Retail, Custom fill value;
      /// </summary>
      byte[]          DigitalProductID;
      /// <summary>
      /// Applies to: Retail, Custom;
      /// </summary>
      int             DistID         = 860; // 0x35c
      ///<summary>
      /// Applies to: All;
      /// </summary>
      string          EXE_Path       = "";
      /// <summary>
      /// Applies to: All;
      /// </summary>
      readonly string InstalledGroup = "1";
      /// <summary>
      /// Applies to: All;
      /// 9 == English
      /// </summary>
      byte            LangID         = 9;
      /// <summary>
      /// Applies to: All
      /// </summary>
      readonly string Launched       = "1";
      /// <summary>
      /// Applies to: Retail, Custom;
      /// No Value
      /// </summary>
      string          PendingVersion = "";
      /// <summary>
      /// Applies to: All;
      /// Only Custom, Retail fill value;
      /// </summary>
      string          PID            = "";
      /// <summary>
      /// Applies to: Retail, Custom, Trial;
      /// Trial is "1". Retail, Custom can be 1.10 or older;
      /// </summary>
      string          Version        = "";
      /// <summary>
      /// If Retail, RetailVersion. Else, TrialVersion.
      /// </summary>
      string          VersionType    = "";
      /// <summary>
      /// Applies to: Retail;
      /// </summary>
      readonly string Zone           = "http://www.zone.com/asp/script/default.asp?Game=Halo&password=Password";

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
    }
  }
}
