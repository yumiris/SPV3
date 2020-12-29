using System;
using System.IO;
using System.Security.Principal;
using System.Diagnostics;
using Microsoft.Win32;
using Enc = System.Text.Encoding;
using WinReg = Microsoft.Win32.Registry;

namespace HXE.HCE
{
  public class Registry
  {
    private static string _dpid   = BogusDPID;
    
    public const string x86       = @"SOFTWARE\Microsoft";
    public const string x86_64    = @"SOFTWARE\WOW6432Node\Microsoft";
    public const string MSG       = "Microsoft Games";
    public const string Retail    = "Halo";
    public const string Custom    = "Halo CE";
    public const string Trial     = "Halo Trial";
    public const string HEK       = "Halo HEK";
    public const string BogusPID  = "00000-000-0000000-00000";
    public const string BogusDPID = "a40000000300000030303030302d3030302d303030303030302d303030303000500000004d30302d30303030300000000000000046203249cdb922e66221b02cc3ee01000000000082610f5b913a8a030000000000000000000000000000000000000000000000003030303030000000000000000b0d0000ba6d6b82000800000000000000000000000000000000000000000000000000000000000000000000f9a404e6";
    public static string PID      = BogusPID;
    public static byte[] ByteDPID = StringToByteArray(StringDPID);
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

    public static bool RunningAsAdmin()
    {
      var Principle = new WindowsPrincipal(WindowsIdentity.GetCurrent());
      return Principle.IsInRole(WindowsBuiltInRole.Administrator);
    }

    public static string WoWCheck()
    {
      return null != WinReg.LocalMachine.OpenSubKey(x86_64) ? x86_64 : x86;
    }

    public static bool GameExists(string game)
    {
      switch(game)
      {
        case "Retail":
          return null == WinReg.LocalMachine.OpenSubKey(Path.Combine(WoWCheck(), MSG, Retail));
        case "Custom":
          return null == WinReg.LocalMachine.OpenSubKey(Path.Combine(WoWCheck(), MSG, Custom));
        case "Trial":
          return null == WinReg.LocalMachine.OpenSubKey(Path.Combine(WoWCheck(), MSG, Trial));
        case "HEK":
          return null == WinReg.LocalMachine.OpenSubKey(Path.Combine(WoWCheck(), MSG, HEK));
        default:
          throw new ArgumentException("The variable passed to GameExists() is invalid.");
      }
    }

    public static void WriteToFile(string game)
    {
      Data data = new Data();
      WriteToFile(game, data);
    }

    public static void WriteToFile(string game, Data data)
    {
      /*Unable to use custom DPIDs*/
      /*Retail and Custom Edition DPIDs end differently*/
      File   file = (File) Path.Combine(Environment.CurrentDirectory, $"{game}.reg");
      string content = "";
      switch (game)
      {
        case "Retail":
          {
            content = 
              "Windows Registry Editor Version 5.00"                                               + '\n' +
                                                                                                     '\n' +
              $@"[HKEY_LOCAL_MACHINE\{WoWCheck()}\{MSG}\{Retail}]"                                 + '\n' +
              $"\"Zone\" = \"{data.Zone}\""                                                        + '\n' +
              "\"DistID\"=dword:0000035c"                                                          + '\n' +
              $"\"Version\"=\"{data.Version}\""                                                    + '\n' +
              "\"Launched\"=\"0\""                                                                 + '\n' +
              $"\"PID\"=\"{data.PID}\""                                                            + '\n' +
              $"\"DigitalProductID\"=hex:a4,20,20,20,03,20,20,20,30,30,30,30,30,2d,30,30,30,2d,\\" + '\n' +
              "  30,30,30,30,30,30,30,2d,30,30,30,30,30,20,50,20,20,20,4d,36,31,2d,30,30,30,\\"    + '\n' +
              "  33,32,20,20,20,20,20,20,20,46,20,32,49,cd,b9,22,e6,62,21,b0,2c,c3,ee,01,20,\\"    + '\n' +
              "  20,20,20,20,95,9c,8a,5e,e6,2e,1e,25,20,20,20,20,20,20,20,20,20,20,20,20,20,\\"    + '\n' +
              "  20,20,20,20,20,20,20,20,20,20,20,38,37,35,30,30,20,20,20,20,20,20,20,01,0d,\\"    + '\n' +
              "  20,20,f0,cf,5d,f1,20,08,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,\\"    + '\n' +
              "  20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,da,a8,75,23"                   + '\n' +
              $"\"EXE Path\"=\"{data.EXE_Path}\""                                                  + '\n' +
              $"\"CDPath\"=\"{data.CDPath}\""                                                      + '\n' +
              "\"VersionType\"=\"RetailVersion\""                                                  + '\n' +
              "\"InstalledGroup\"=\"1\""                                                           + '\n' +
              "\"LangID\"=dword:00000009"                                                          + '\n' +
              "\"PendingVersion\"=\"\""                                                            + '\n' +
                                                                                                     '\n' +
                                                                                                     '\n';
          }
          break;
        case "Custom":
          {
            content = 
              "Windows Registry Editor Version 5.00"                                               + "\r\n" +
                                                                                                     "\r\n" +
              $@"[HKEY_LOCAL_MACHINE\{WoWCheck()}\{MSG}\{Custom}]"                                 + "\r\n" +
              "\"DistID\"=dword:0000035c"                                                          + "\r\n" +
              $"\"Version\"=\"{data.Version}\""                                                    + "\r\n" +
              "\"Launched\"=\"0\""                                                                 + "\r\n" +
              $"\"PID\"=\"{data.PID}\""                                                            + "\r\n" +
              "\"DigitalProductID\"=hex:a4,00,00,00,03,00,00,00,30,30,30,30,30,2d,30,30,30,2d,\\"  + "\r\n" +
              "  30,30,30,30,30,30,30,2d,30,30,30,30,30,00,50,00,00,00,4d,30,30,2d,30,30,30,\\"    + "\r\n" +
              "  30,30,00,00,00,00,00,00,00,46,20,32,49,cd,b9,22,e6,62,21,b0,2c,c3,ee,01,00,\\"    + "\r\n" +
              "  00,00,00,00,82,61,0f,5b,91,3a,8a,03,00,00,00,00,00,00,00,00,00,00,00,00,00,\\"    + "\r\n" +
              "  00,00,00,00,00,00,00,00,00,00,00,30,30,30,30,30,00,00,00,00,00,00,00,0b,0d,\\"    + "\r\n" +
              "  00,00,ba,6d,6b,82,00,08,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,\\"    + "\r\n" +
              "  00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,f9,a4,04,e6"                   + "\r\n" +
              $"\"EXE Path\"=\"{data.EXE_Path}\""                                                  + "\r\n" +
              $"\"CDPath\"=\"{data.CDPath}\""                                                      + "\r\n" +
              "\"VersionType\"=\"TrialVersion\""                                                   + "\r\n" +
              "\"InstalledGroup\"=\"1\""                                                           + "\r\n" +
              "\"LangID\"=dword:00000009"                                                          + "\r\n" +
              "\"PendingVersion\"=\"\""                                                            + "\r\n" +
                                                                                                     "\r\n" +
                                                                                                     "\r\n";
          }
          break;
        case "Trial":
          {
            content = 
              "Windows Registry Editor Version 5.00"                                               + "\r\n" +
                                                                                                     "\r\n" +
              $@"[HKEY_LOCAL_MACHINE\{WoWCheck()}\{MSG}\{Trial}]"                                  + "\r\n" +
              "\"Version\"=\"1\""                                                                  + "\r\n" +
              "\"Launched\"=\"0\""                                                                 + "\r\n" +
              "\"PID\"=\"\""                                                                       + "\r\n" +
              "\"DigitalProductID\"=hex:"                                                          + "\r\n" +
              $"\"EXE Path\"=\"{data.EXE_Path}\""                                                  + "\r\n" +
              $"\"CDPath\"=\"{data.CDPath}\""                                                      + "\r\n" +
              "\"VersionType\"=\"TrialVersion\""                                                   + "\r\n" +
              "\"InstalledGroup\"=\"1\""                                                           + "\r\n" +
              "\"LangID\"=dword:00000009"                                                          + "\r\n" +
                                                                                                     "\r\n" +
                                                                                                     "\r\n";
          }
          break;
        case "HEK":
          {
            content = 
              "Windows Registry Editor Version 5.00"                                               + "\r\n" +
                                                                                                     "\r\n" +
              $@"[HKEY_LOCAL_MACHINE\{WoWCheck()}\{MSG}\{HEK}]"                                    + "\r\n" +
              "\"Launched\"=\"0\""                                                                 + "\r\n" +
              "\"PID\"=\"\""                                                                       + "\r\n" +
              "\"DigitalProductID\"=hex:"                                                          + "\r\n" +
              $"\"EXE Path\"=\"{data.EXE_Path}\""                                                  + "\r\n" +
              $"\"CDPath\"=\"{data.CDPath}\""                                                      + "\r\n" +
              "\"VersionType\"=\"TrialVersion\""                                                   + "\r\n" +
              "\"InstalledGroup\"=\"1\""                                                           + "\r\n" +
              "\"LangID\"=dword:00000009"                                                          + "\r\n" +
                                                                                                     "\r\n" +
                                                                                                     "\r\n";
          }
          break;
        default:
          break;
      } 
      file.WriteAllText(content);
    }

    /// <summary>
    /// Creates the registry keys for the given game. Uses default Data values for registry variables. 
    /// </summary>
    /// <remarks>Totally broken. Use WriteToFile() instead.</remarks>

    public static void CreateKeys(string game)
    {
      Data data = new Data();
      CreateKeys(game, data);
    }

    /// <summary>
    /// Creates the registry keys for the given game. 
    /// Requires an instance of Registry.Data which can be used for specifying values of registry variables such as the EXE path.
    /// </summary>
    /// <param name="game">The game or app.</param>
    /// <param name="path">The Registry key path.</param>
    /// <param name="data">An instance of the Registry.Data class.</param>
    /// <remarks>TOTALLY BROKEN. Use WriteToFile() instead.</remarks>
    public static void CreateKeys(string game, Data data)
    {
      /** Temporarily Abandoned
       * This method of writing to the Windows Registry
       * was intended to either...
       * A. Temporarily elevate the current process to write
       *    directly to the Registry. (NOT POSSIBLE)
       *    or...
       * B. Pass an instance of Data to a new, elevated 
       *    process so the process can then write that 
       *    Data to the Registry.
       *    
       *    Because data cannot be directly passed between 
       *    elevated and non-elevated processes, this idea
       *    is limited to two implementations:
       *    > Pass Data via start parameters to a new, 
       *      elevated HXE process.
       *      or...
       *    > Write Data to a file and read that file in
       *      the new process.
       */

      if (!RunningAsAdmin())
      {
        var StartInfo = new ProcessStartInfo
        {
          WorkingDirectory = Environment.CurrentDirectory,
          FileName = Process.GetCurrentProcess().MainModule.FileName,
          UseShellExecute = true,
          Verb = "runas"
          /** runas rundown
            * "runas" implicitly means "Run As Administrator". 
            * This particular Verb requires UseShellExecute to be True.
            * As it is with many string variables and parameters, acceptable
            *  values are poorly documented.
            * Microsoft Docs explains the acceptable values depend on the file 
            *  extension of the process' file name.
            * The only examples it provides are valid for .txt and suggests using 
            *  StartInfo.Verbs to print/list acceptable values for the given file name.
            * See https://stackoverflow.com/a/133500
            */
        };
        using (Process process = Process.Start(StartInfo))
        {
          int timeout = 5000;
          process.WaitForExit(timeout);
          if (process.ExitCode != 0)
            throw new Exception("Elevated process exited unexpectedly. Exit Code: " + process.ExitCode);
        }
      }

      var key = WinReg.LocalMachine.OpenSubKey(Path.Combine(WoWCheck(), MSG), true);
      /// Create the game's registry key if it doesn't exist.
      if (key == null)
      {
        var key2 = WinReg.LocalMachine.OpenSubKey(Path.Combine(WoWCheck(), MSG), true);
        if (key2 == null)
        {
          WinReg.LocalMachine.OpenSubKey(WoWCheck(), true).CreateSubKey(MSG);
        }
        key2.CreateSubKey(game);
      }
      switch (game)
      {
        case "Retail":
          {
            key = WinReg.LocalMachine.OpenSubKey(Path.Combine(WoWCheck(), MSG, Retail));
            
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
            key = WinReg.LocalMachine.OpenSubKey(Path.Combine());
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
            key = WinReg.LocalMachine.OpenSubKey(Path.Combine());
            data.DigitalProductID = null;
            data.PID              = null;
            data.Version          = "1";
            data.VersionType      = "TrialVersion";
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
            key = WinReg.LocalMachine.OpenSubKey(Path.Combine());
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
    /// <remarks>Normally, I would use a more efficients variable than string, but string allows better labeling</remarks>
    public static void GetRegistryKeys(string game)
    {
      Data data = new Data();
      GetRegistryKeys(game, data);
    }

    public static void GetRegistryKeys(string game, Data data)
    {
      string path = Path.Combine(WoWCheck(), MSG);
      RegistryKey key;

      try
      {
        switch (game)
        {
          case "Retail":
            path = Path.Combine(path, Custom);
            key = WinReg.LocalMachine.OpenSubKey(path);
            if (key != null)// read to Data
            {
              data.CDPath           = key.GetValue("CDPath"          , data.CDPath          ).ToString();
              data.DigitalProductID = Enc.Unicode.GetBytes(
                                      key.GetValue("DigitalProductID", data.DigitalProductID).ToString());
              data.EXE_Path         = key.GetValue("EXE Path"        , data.EXE_Path        ).ToString();
              data.LangID           = Enc.Unicode.GetBytes(
                                      key.GetValue("LangID"          , data.LangID          ).ToString())[0];
              data.Launched         = key.GetValue("Launched"        , data.Launched        ).ToString();
              data.PendingVersion   = key.GetValue("PendingVersion"  , data.PendingVersion  ).ToString();
              data.PID              = key.GetValue("PID"             , data.PID             ).ToString();
              data.Version          = key.GetValue("Version"         , data.Version         ).ToString();
              data.VersionType      = "RetailVersion";
            }
            break;
          case "Custom":
            path = Path.Combine(path, Custom);
            key = WinReg.LocalMachine.OpenSubKey(path);
            if (key != null) // read to Data
            {
              data.CDPath           = key.GetValue("CDPath"          , data.CDPath          ).ToString();
              data.DigitalProductID = Enc.Unicode.GetBytes(
                                      key.GetValue("DigitalProductID", data.DigitalProductID).ToString());
              data.EXE_Path         = key.GetValue("EXE Path"        , data.EXE_Path        ).ToString();
              data.LangID           = Enc.Unicode.GetBytes(
                                      key.GetValue("LangID"          , data.LangID          ).ToString())[0];
              data.Launched         = key.GetValue("Launched"        , data.Launched        ).ToString();
              data.PendingVersion   = key.GetValue("PendingVersion"  , data.PendingVersion  ).ToString();
              data.PID              = key.GetValue("PID"             , data.PID             ).ToString();
              data.Version          = key.GetValue("Version"         , data.Version         ).ToString();
              data.VersionType      = "TrialVersion";
            }
            break;
          case "Trial":
            path = Path.Combine(path, Trial);
            key = WinReg.LocalMachine.OpenSubKey(path);
            if (key != null) // read to Data
            {
              data.CDPath           = key.GetValue("CDPath"          , data.CDPath          ).ToString();
              data.DigitalProductID = Enc.Unicode.GetBytes(
                                      key.GetValue("DigitalProductID", data.DigitalProductID).ToString());
              data.EXE_Path         = key.GetValue("EXE Path"        , data.EXE_Path        ).ToString();
              data.LangID           = Enc.Unicode.GetBytes(
                                      key.GetValue("LangID"          , data.LangID          ).ToString())[0];
              data.Launched         = key.GetValue("Launched"        , data.Launched        ).ToString();
              data.PID              = key.GetValue("PID"             , data.PID             ).ToString();
              data.Version          = key.GetValue("Version"         , data.Version         ).ToString();
              data.VersionType      = "TrialVersion";

            }
            break;
          case "HEK":
            path = Path.Combine(path, HEK);
            key = WinReg.LocalMachine.OpenSubKey(path);
            if (key != null) // read to Data
            {
              data.CDPath           = key.GetValue("CDPath"  , data.CDPath  ).ToString();
              data.DigitalProductID = null;
              data.EXE_Path         = key.GetValue("EXE Path", data.EXE_Path).ToString();
              data.LangID           = Enc.Unicode.GetBytes(
                                      key.GetValue("LangID"  , data.LangID  ).ToString())[0];
              data.Launched         = key.GetValue("Launched", data.Launched).ToString();
              data.PID              = null;
              data.VersionType      = "TrialVersion";
            }
            break;
          default:
            break;
        }
      }
      catch(Exception e)
      {
        throw new Exception("Failed to read registry keys: " + e);
      }
    }

    public static void WriteKey(Data data)
    {
      //take a parameter
    }

    /// <summary>
    /// A Class object to store a game's registry key variable and values in memory.
    /// </summary>
    public class Data
    {
      /** ---- Halo 1 Registry Keys ----
       *  Retail : Halo
       *  Custom : Halo CE
       *  Trial  : Halo Trial
       *  HEK    : Halo HEK
       */

      /** ---- SubKey Values and Rules ---- 
       *  PLEASE refer to the examples below this section for rules. 
       *  Don't assign incompatible values!
       */
      public string          CDPath           = $@"{Environment.CurrentDirectory}";
      public byte[]          DigitalProductID = ByteDPID;
      public readonly int    DistID           = 860; // 0x35c
      public string          EXE_Path         = "";
      public readonly string InstalledGroup   = "1";
      public byte            LangID           = 9;
      public string          Launched         = "0";
      public string          PendingVersion   = "";
      public string          PID              = Registry.PID;
      public string          Version          = "";
      public string          VersionType      = "";
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
