using System.IO;

namespace Atarashii
{
    /// <summary>
    ///     Package
    /// </summary>
    public class Package
    {
        public const string Directory = "Packages";

        public Package(string name, string description, string destination)
        {
            Name = name;
            Description = description;
            Destination = destination;
        }

        public string Name { get; }
        public string Description { get; }
        public string Destination { get; }

        public void Install()
        {
            if (File.Exists(Path.Combine(Directory, Name)))
                throw new FileNotFoundException($"{Name} does not exist in {Directory}");
        }
    }
}