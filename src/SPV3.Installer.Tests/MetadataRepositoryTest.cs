using System;
using System.Collections.Generic;
using NUnit.Framework;
using SPV3.Domain;
using Version = SPV3.Domain.Version;

namespace SPV3.Installer.Tests
{
    [TestFixture]
    public class MetadataRepositoryTest
    {
        private static readonly Metadata Metadata = new Metadata
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
            var repository = new MetadataRepository((File) file);

            repository.Save(Metadata);
            var loadedMetadata = repository.Load();

            Assert.AreEqual((string) Metadata.Version, (string) loadedMetadata.Version);

            System.IO.File.Delete(file);
        }

        [Test]
        public void Repository_MetadataPackageIsSaved()
        {
            var file = Guid.NewGuid().ToString();
            var repository = new MetadataRepository((File) file);

            repository.Save(Metadata);
            var loadedMetadata = repository.Load();

            for (var i = 0; i < Metadata.Packages.Count; i++)
                Assert.AreEqual(Metadata.Packages[i].Name.Value, loadedMetadata.Packages[i].Name.Value);

            System.IO.File.Delete(file);
        }
    }
}