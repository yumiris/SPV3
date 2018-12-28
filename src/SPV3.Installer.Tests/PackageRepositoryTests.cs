using System;
using System.Collections.Generic;
using NUnit.Framework;
using SPV3.Domain;

namespace SPV3.Installer.Tests
{
    [TestFixture]
    public class PackageRepositoryTests
    {
        private static readonly Package Package = new Package
        {
            Name = (Name) "Main",
            Entries = new List<Entry>
            {
                (Entry) "Flandre Scarlet",
                (Entry) "Reimu Hakurei",
                (Entry) "Yuuka Kazami",
                (Entry) "Marisa Kirisame",
                (Entry) "Yukari Yakumo",
                (Entry) "Utsuho Reiuji",
                (Entry) "Remilia Scarlet"
            }
        };

        [Test]
        public void Repository_PackageEntryIsSaved()
        {
            var file = Guid.NewGuid().ToString();
            var repository = new PackageRepository((File) file);
            repository.Save(Package);
            var loadedPackage = repository.Load();
            System.IO.File.Delete(file);

            for (var i = 0; i < Package.Entries.Count; i++)
                Assert.AreEqual(Package.Entries[i].Name.Value, loadedPackage.Entries[i].Name.Value);
        }

        [Test]
        public void Repository_PackageNameIsSaved()
        {
            var file = Guid.NewGuid().ToString();
            var repository = new PackageRepository((File) file);
            repository.Save(Package);
            var loadedPackage = repository.Load();
            System.IO.File.Delete(file);

            Assert.AreEqual(Package.Name.Value, loadedPackage.Name.Value);
        }
    }
}