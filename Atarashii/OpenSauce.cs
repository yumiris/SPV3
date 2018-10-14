using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Atarashii.Exceptions;

namespace Atarashii
{
    /// <summary>
    ///     OpenSauce installation and configuration representative.
    /// </summary>
    public class OpenSauce
    {
        public CacheFiles CacheFiles { get; set; } = new CacheFiles();

        public Rasterizer Rasterizer { get; set; } = new Rasterizer();

        public Camera Camera { get; set; } = new Camera();

        public Networking Networking { get; set; } = new Networking();

        public Objects Objects { get; set; } = new Objects();

        [XmlElement(ElementName = "HUD")] public Hud Hud { get; set; } = new Hud();

        /// <summary>
        ///     Converts this instance to an XML string.
        /// </summary>
        /// <returns>
        ///     XML representation of this instance.
        /// </returns>
        public string ToXml()
        {
            using (var writer = new StringWriter())
            {
                var serialiser = new XmlSerializer(typeof(OpenSauce));
                serialiser.Serialize(writer, this);
                return writer.ToString();
            }
        }

        /// <summary>
        ///     Installs the OpenSauce libraries to the given HCE directory path.
        /// </summary>
        /// <param name="hcePath">
        ///     A valid HCE directory path.
        /// </param>
        /// <param name="logger">
        ///    Logging instance to be injected into each Package instance's Install() method.
        /// </param>
        /// <exception cref="OpenSauceException">
        ///     Invalid HCE directory path.
        ///     - or -
        ///     Target directory does not exist.
        ///     - or -
        ///     Package does not exist.
        /// </exception>
        /// <exception cref="OpenSauceException"></exception>
        public void InstallTo(string hcePath, ILogger logger)
        {
            if (!File.Exists(Path.Combine(hcePath, Executable.Name)))
                throw new OpenSauceException("Invalid HCE directory path.");

            if (!Directory.Exists(hcePath))
                throw new OpenSauceException("Target directory does not exist.");

            string guiDirPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string usrDirPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var packages = new List<Package>
            {
                new Package("lib.pkg", "OpenSauce core and dependencies", hcePath),
                new Package("gui.pkg", "In-game OpenSauce UI assets", guiDirPath),
                new Package("usr.pkg", "OpenSauce XML user configuration", usrDirPath)
            };

            foreach (var package in packages)
                if (!File.Exists(package.ArchiveName))
                    throw new OpenSauceException("Package does not exist.");

            foreach (var package in packages)
            {
                package.Install(logger);
            }
        }
    }

    public class CacheFiles
    {
        public bool CheckYeloFilesFirst { get; set; } = true;
    }

    public class Rasterizer
    {
        [XmlElement(ElementName = "GBuffer")] public Gbuffer Gbuffer { get; set; } = new Gbuffer();

        public Upgrades Upgrades { get; set; } = new Upgrades();

        public ShaderExtensions ShaderExtensions { get; set; } = new ShaderExtensions();

        public PostProcessing PostProcessing { get; set; } = new PostProcessing();
    }

    public class Gbuffer
    {
        public bool Enabled { get; set; } = true;
    }

    public class Upgrades
    {
        public bool MaximumRenderedTriangles { get; set; } = true;
    }

    public class ShaderExtensions
    {
        public bool Enabled = true;

        [XmlElement(ElementName = "Rasterizer")]
        public ShaderRasterizer ShaderRasterizer { get; set; } = new ShaderRasterizer();
    }

    public class ShaderRasterizer
    {
        [XmlElement(ElementName = "ShaderExtensions")]
        public RasterizerShaderExtensions RasterizerShaderExtensions { get; set; } = new RasterizerShaderExtensions();
    }

    public class RasterizerShaderExtensions
    {
        [XmlElement(ElementName = "Object")] public ShaderObject ShaderObject { get; set; } = new ShaderObject();

        [XmlElement(ElementName = "Environment")]
        public ShaderEnvironment ShaderEnvironment { get; set; } = new ShaderEnvironment();

        public Effect Effect { get; set; } = new Effect();
    }

    public class ShaderObject
    {
        public bool NormalMaps { get; set; } = true;

        public bool DetailNormalMaps { get; set; } = true;

        public bool SpecularMaps { get; set; } = true;

        public bool SpecularLighting { get; set; } = true;
    }

    public class ShaderEnvironment
    {
        public bool DiffuseDirectionalLightmaps { get; set; } = true;

        public bool SpecularDirectionalLightmaps { get; set; } = true;
    }

    public class Effect
    {
        public bool DepthFade { get; set; } = true;
    }

    public class PostProcessing
    {
        public MotionBlur MotionBlur { get; set; } = new MotionBlur();

        public Bloom Bloom { get; set; } = new Bloom();

        public AntiAliasing AntiAliasing { get; set; } = new AntiAliasing();

        public ExternalEffects ExternalEffects { get; set; } = new ExternalEffects();

        public MapEffects MapEffects { get; set; } = new MapEffects();
    }

    public class MotionBlur
    {
        public bool Enabled { get; set; } = true;

        public decimal BlurAmount { get; set; } = 1;
    }

    public class Bloom
    {
        public bool Enabled { get; set; } = true;
    }

    public class AntiAliasing
    {
        public bool Enabled { get; set; } = true;
    }

    public class ExternalEffects
    {
        public bool Enabled { get; set; } = true;
    }

    public class MapEffects
    {
        public bool Enabled { get; set; } = true;
    }

    public class Camera
    {
        public double FieldOfView { get; set; } = 70.0;

        public bool IgnoreFOVChangeInCinematics { get; set; } = true;

        public bool IgnoreFOVChangeInMainMenu { get; set; } = true;
    }

    public class Networking
    {
        public GameSpy GameSpy { get; set; } = new GameSpy();

        public MapDownload MapDownload { get; set; } = new MapDownload();
    }

    public class GameSpy
    {
        public bool NoUpdateCheck { get; set; } = true;
    }

    public class MapDownload
    {
        public bool Enabled { get; set; } = true;
    }

    public class Objects
    {
        public bool VehicleRemapperEnabled { get; set; } = true;
    }

    public class Hud
    {
        [XmlElement(ElementName = "ShowHUD")] public bool ShowHud { get; set; } = true;

        [XmlElement(ElementName = "ScaleHUD")] public bool ScaleHud { get; set; } = true;

        [XmlElement(ElementName = "HUDScale")] public HudScale HudScale { get; set; } = new HudScale();
    }

    public class HudScale
    {
        public float X { get; set; } = 1;
        public float Y { get; set; } = 1;
    }
}