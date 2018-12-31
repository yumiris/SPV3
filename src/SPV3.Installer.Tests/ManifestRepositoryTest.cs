/**
 * Copyright (c) 2018 Emilian Roman
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

using System;
using System.Collections.Generic;
using NUnit.Framework;
using SPV3.Domain;
using SPV3.Installer.Domain;
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
    }
}