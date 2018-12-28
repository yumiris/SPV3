using System.IO;
using NUnit.Framework;

namespace SPV3.Domain.Tests
{
    [TestFixture]
    public class PackageRepositoryTests
    {
        private const string TestFile = "data.bin";

        private static readonly PackageRepository _repository = new PackageRepository((File) TestFile);

        private static readonly Package _package = new Package
        {
            Name = (Name) "Main",
            Entries = new System.Collections.Generic.List<Entry>
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

        [SetUp]
        public void SetUp()
        {
            _repository.Save(_package);
        }

        [Test]
        public void Repository_PackageNameIsSaved_True()
        {
            var loadedPackage = _repository.Load();
            Assert.AreEqual(_package.Name.Value, loadedPackage.Name.Value);
        }

        [Test]
        public void Repository_PackageEntryIsSaved_True()
        {
            var loadedPackage = _repository.Load();

            for (var i = 0; i < _package.Entries.Count; i++)
                Assert.AreEqual(_package.Entries[i].Name.Value, loadedPackage.Entries[i].Name.Value);
        }

        [TearDown]
        public void TearDown()
        {
            System.IO.File.Delete(TestFile);
        }
    }
}