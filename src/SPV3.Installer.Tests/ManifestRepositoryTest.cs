using System;
using System.Collections.Generic;
using NUnit.Framework;
using SPV3.Domain;
using Version = SPV3.Domain.Version;

namespace SPV3.Installer.Tests
{
    [TestFixture]
    public class ManifestRepositoryTest
    {
        private static readonly Manifest Manifest = new Manifest
        {
            Version = (Version) "2.4.8",
            Packages = new List<Package>
            {
                new Package
                {
                    Name = (Name) "0x00",
                    Entries = new List<Entry>
                    {
                        (Entry) "Flandre Scarlet",
                        (Entry) "Reimu Hakurei"
                    }
                },
                new Package
                {
                    Name = (Name) "0x01",
                    Entries = new List<Entry>
                    {
                        (Entry) "Yuuka Kazami",
                        (Entry) "Marisa Kirisame"
                    }
                },
                new Package
                {
                    Name = (Name) "0x02",
                    Entries = new List<Entry>
                    {
                        (Entry) "Yukari Yakumo",
                        (Entry) "Utsuho Reiuji",
                        (Entry) "Remilia Scarlet"
                    }
                }
            }
        };

        [Test]
        public void Repository_MetadataVersionIsSaved()
        {
            var file = Guid.NewGuid().ToString();
            var repository = new ManifestRepository((File) file);

            repository.Save(Manifest);
            var loadedMetadata = repository.Load();

            Assert.AreEqual((string) Manifest.Version, (string) loadedMetadata.Version);

            System.IO.File.Delete(file);
        }

        [Test]
        public void Repository_MetadataPackageIsSaved()
        {
            var file = Guid.NewGuid().ToString();
            var repository = new ManifestRepository((File) file);

            repository.Save(Manifest);
            var loadedMetadata = repository.Load();

            for (var i = 0; i < Manifest.Packages.Count; i++)
                Assert.AreEqual(Manifest.Packages[i].Name.Value, loadedMetadata.Packages[i].Name.Value);

            System.IO.File.Delete(file);
        }
    }
}