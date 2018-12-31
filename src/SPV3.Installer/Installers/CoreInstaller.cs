using System.IO.Compression;
using System.Linq;
using SPV3.Domain;

namespace SPV3.Installer.Installers
{
    /// <inheritdoc />
    public class CoreInstaller : Common.Installer
    {
        /// <summary>
        ///     Name for the core package.
        /// </summary>
        private const string CorePackage = "0x01.bin";

        /// <inheritdoc />
        public CoreInstaller(Directory target, Directory backup, IStatus status = null) : base(target, backup, status)
        {
            //
        }

        /// <inheritdoc />
        public override void Install(Manifest manifest)
        {
            Notify("----------------------------");
            Notify("Invoked core installation...");
            Notify("----------------------------");

            Notify("Running pre-install tasks...");
            Migrate(manifest.Packages.Single(package => package.Name == CorePackage));

            Notify("Installing the core files...");
            ZipFile.ExtractToDirectory(CorePackage, Target);

            Notify("----------------------------");
            Notify("Resolve core installation...");
            Notify("----------------------------");
        }
    }
}