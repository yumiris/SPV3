using System.IO;

namespace Atarashii
{
    /// <summary>
    ///     Archive installer and verifier.
    /// </summary>
    public class Package
    {
        public const string Directory = "Packages";

        public string ArchiveName { get; }
        public string Description { get; }
        public string Destination { get; }

        public Package(string archiveName, string description, string destination)
        {
            ArchiveName = archiveName;
            Description = description;
            Destination = destination;
        }

        public void Install()
        {
            if (!File.Exists(Path.Combine(Directory, ArchiveName)))
                throw new PackageException("Cannot install specified package. Archive does not exist.");
        }
    }
}