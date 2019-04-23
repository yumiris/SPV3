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
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SPV3.CLI
{
  /// <summary>
  ///   Object representing an OpenSauce user configuration.
  /// </summary>
  public class OpenSauce : File
  {
    public OpenSauceCacheFiles CacheFiles { get; set; } = new OpenSauceCacheFiles();
    public OpenSauceRasterizer Rasterizer { get; set; } = new OpenSauceRasterizer();
    public OpenSauceCamera     Camera     { get; set; } = new OpenSauceCamera();
    public OpenSauceNetworking Networking { get; set; } = new OpenSauceNetworking();
    public OpenSauceHUD        HUD        { get; set; } = new OpenSauceHUD();
    public OpenSauceObjects    Objects    { get; set; } = new OpenSauceObjects();

    /// <summary>
    ///   Saves object state to the inbound file.
    /// </summary>
    public void Save()
    {
      using (var writer = new StringWriter())
      {
        var serialiser = new XmlSerializer(typeof(OpenSauce));
        serialiser.Serialize(writer, this);
        WriteAllText(writer.ToString());
      }
    }

    /// <summary>
    ///   Loads object state from the inbound file.
    /// </summary>
    public void Load()
    {
      using (var reader = new StringReader(ReadAllText()))
      {
        var serialiser = new XmlSerializer(typeof(OpenSauce));
        var serialised = (OpenSauce) serialiser.Deserialize(reader);

        CacheFiles = serialised.CacheFiles;
        Rasterizer = serialised.Rasterizer;
        Camera     = serialised.Camera;
        Networking = serialised.Networking;
        HUD        = serialised.HUD;
        Objects    = serialised.Objects;
      }
    }

    /// <summary>
    ///   Represents the inbound object as a string.
    /// </summary>
    /// <param name="openSauce">
    ///   Object to represent as string.
    /// </param>
    /// <returns>
    ///   String representation of the inbound object.
    /// </returns>
    public static implicit operator string(OpenSauce openSauce)
    {
      return openSauce.Path;
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
    public static explicit operator OpenSauce(string name)
    {
      return new OpenSauce
      {
        Path = name
      };
    }

    public class OpenSauceCacheFiles
    {
      public bool CheckYeloFilesFirst { get; set; } = true;
    }

    public class OpenSauceRasterizer
    {
      public RasterizerGBuffer          GBuffer          { get; set; } = new RasterizerGBuffer();
      public RasterizerUpgrades         Upgrades         { get; set; } = new RasterizerUpgrades();
      public RasterizerShaderExtensions ShaderExtensions { get; set; } = new RasterizerShaderExtensions();
      public RasterizerPostProcessing   PostProcessing   { get; set; } = new RasterizerPostProcessing();

      public class RasterizerGBuffer
      {
        public bool Enabled { get; set; } = true;
      }

      public class RasterizerUpgrades
      {
        public bool MaximumRenderedTriangles { get; set; } = true;
      }

      public class RasterizerShaderExtensions
      {
        public bool                        Enabled     { get; set; } = true;
        public ShaderExtensionsObject      Object      { get; set; } = new ShaderExtensionsObject();
        public ShaderExtensionsEnvironment Environment { get; set; } = new ShaderExtensionsEnvironment();
        public ShaderExtensionsEffect      Effect      { get; set; } = new ShaderExtensionsEffect();

        public class ShaderExtensionsObject
        {
          public bool NormalMaps       { get; set; } = true;
          public bool DetailNormalMaps { get; set; } = true;
          public bool SpecularMaps     { get; set; } = true;
          public bool SpecularLighting { get; set; } = true;
        }

        public class ShaderExtensionsEnvironment
        {
          public bool DiffuseDirectionalLightmaps  { get; set; } = true;
          public bool SpecularDirectionalLightmaps { get; set; } = true;
        }

        public class ShaderExtensionsEffect
        {
          public bool DepthFade { get; set; } = true;
        }
      }

      public class RasterizerPostProcessing
      {
        public PostProcessingMotionBlur      MotionBlur      { get; set; } = new PostProcessingMotionBlur();
        public PostProcessingBloom           Bloom           { get; set; } = new PostProcessingBloom();
        public PostProcessingAntiAliasing    AntiAliasing    { get; set; } = new PostProcessingAntiAliasing();
        public PostProcessingExternalEffects ExternalEffects { get; set; } = new PostProcessingExternalEffects();
        public PostProcessingMapEffects      MapEffects      { get; set; } = new PostProcessingMapEffects();

        public class PostProcessingMotionBlur
        {
          public bool   Enabled    { get; set; } = true;
          public double BlurAmount { get; set; } = 1.00;
        }

        public class PostProcessingBloom
        {
          public bool Enabled { get; set; } = true;
        }

        public class PostProcessingAntiAliasing
        {
          public bool Enabled { get; set; } = false;
        }

        public class PostProcessingExternalEffects
        {
          public bool Enabled { get; set; } = true;
        }

        public class PostProcessingMapEffects
        {
          public bool Enabled { get; set; } = true;
        }
      }
    }

    public class OpenSauceCamera
    {
      public double FieldOfView                 { get; set; } = 70.00;
      public bool   IgnoreFOVChangeInCinematics { get; set; } = true;
      public bool   IgnoreFOVChangeInMainMenu   { get; set; } = true;
    }

    public class OpenSauceNetworking
    {
      public NetworkingGameSpy      GameSpy      { get; set; } = new NetworkingGameSpy();
      public NetworkingMapDownload  MapDownload  { get; set; } = new NetworkingMapDownload();
      public NetworkingVersionCheck VersionCheck { get; set; } = new NetworkingVersionCheck();

      public class NetworkingGameSpy
      {
        public bool NoUpdateCheck { get; set; } = true;
      }

      public class NetworkingMapDownload
      {
        public bool Enabled { get; set; } = true;
      }

      public class NetworkingVersionCheck
      {
        public VersionCheckDate       Date       { get; set; } = new VersionCheckDate();
        public VersionCheckServerList ServerList { get; set; } = new VersionCheckServerList();

        public class VersionCheckDate
        {
          public int Day   { get; set; } = (int) DateTime.Now.DayOfWeek;
          public int Month { get; set; } = DateTime.Now.Month;
          public int Year  { get; set; } = DateTime.Now.Year;
        }

        public class VersionCheckServerList
        {
          public int    Version { get; set; } = 1;
          public string Server  { get; set; } = "http://os.halomods.com/Halo1/CE/Halo1_CE_Version.xml";
        }
      }
    }

    public class OpenSauceObjects
    {
      public bool          VehicleRemapperEnabled { get; set; } = true;
      public ObjectsWeapon Weapon                 { get; set; } = new ObjectsWeapon();

      public class ObjectsWeapon
      {
        public WeaponPositions Positions { get; set; } = new WeaponPositions();

        public class WeaponPositions
        {
          public List<PositionWeapon> Weapon { get; set; } = new List<PositionWeapon>();

          [XmlType("Weapon")]
          public class PositionWeapon
          {
            public string         Name     { get; set; } = string.Empty;
            public WeaponPosition Position { get; set; } = new WeaponPosition();

            public class WeaponPosition
            {
              public long I { get; set; } = 0;
              public long J { get; set; } = 0;
              public long K { get; set; } = 0;
            }
          }
        }
      }
    }

    public class OpenSauceHUD
    {
      public bool        ShowHUD  { get; set; }
      public bool        ScaleHUD { get; set; }
      public HUDHUDScale HUDScale { get; set; }

      public class HUDHUDScale
      {
        public long X { get; set; } = 0;
        public long Y { get; set; } = 0;
      }
    }
  }
}