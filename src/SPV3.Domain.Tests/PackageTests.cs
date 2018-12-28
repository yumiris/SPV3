using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace SPV3.Domain.Tests
{
    public class PackageTests
    {
        [Test]
        public void EntriesCount_ExceedsUpperBound_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var list = new List<Entry>();

                for (var i = 0; i < 256; i++)
                    list.Add((Entry) "Not a cat girl, but a werewolf.");

                new Package
                {
                    Entries = list
                };
            });
        }

        [Test]
        public void Repository_DataIsSaved_True()
        {
            var sourcePackage = new Package
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
            
            var packageRepository = new PackageRepository("data.bin");
            packageRepository.Save(sourcePackage);

            var loadedPackage = packageRepository.Load();
            Assert.AreEqual(sourcePackage.Name.Value, loadedPackage.Name.Value);
            
            File.Delete("data.bin");
        }
    }
}