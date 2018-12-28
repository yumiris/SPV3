using System.Collections.Generic;
using NUnit.Framework;

namespace SPV3.Domain.Tests
{
    [TestFixture]
    public class PackageRepositoryTests
    {
        [SetUp]
        public void SetUp()
        {
            Repository.Save(Package);
        }

        [TearDown]
        public void TearDown()
        {
            System.IO.File.Delete(TestFile);
        }

        private const string TestFile = "data.bin";

        private static readonly PackageRepository Repository = new PackageRepository((File) TestFile);

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
        public void Repository_PackageEntryIsSaved_True()
        {
            var loadedPackage = Repository.Load();

            for (var i = 0; i < Package.Entries.Count; i++)
                Assert.AreEqual(Package.Entries[i].Name.Value, loadedPackage.Entries[i].Name.Value);
        }

        [Test]
        public void Repository_PackageNameIsSaved_True()
        {
            var loadedPackage = Repository.Load();
            Assert.AreEqual(Package.Name.Value, loadedPackage.Name.Value);
        }
    }
}