using SPV3.Domain;

namespace SPV3.Installer.Installers
{
    /// <inheritdoc />
    public class MetaInstaller : Common.Installer
    {
        public MetaInstaller(Directory target, Directory backup, IStatus status = null) : base(target, backup, status)
        {
            //
        }

        /// <inheritdoc />
        public override void Install(Manifest manifest)
        {
            Notify("============================");
            Notify("Initiated install routine...");
            Notify("============================");

            new CoreInstaller(Target, Backup, Status).Install(manifest);
            new DataInstaller(Target, Backup, Status).Install(manifest);

            Notify("============================");
            Notify("Completed install routine...");
            Notify("============================");
        }
    }
}