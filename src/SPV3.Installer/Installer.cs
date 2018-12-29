using System.IO.Compression;
using System.Windows.Controls;
using SPV3.Domain;
using SPV3.Installer.Data;

namespace SPV3.Installer
{
    public class Installer
    {
        private Directory _target;

        public Installer(Directory target)
        {
            _target = target;
        }

        public void Install(Label label)
        {
            var manifest = new ManifestRepository((File) "0x00.bin").Load();

            foreach (var package in manifest.Packages)
            {
                ZipFile.ExtractToDirectory(package.Name, _target);
            }
        }
    }
}