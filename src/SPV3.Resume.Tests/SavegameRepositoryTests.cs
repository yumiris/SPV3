using NUnit.Framework;
using SPV3.Domain;

namespace SPV3.Resume.Tests
{
    [TestFixture]
    public class SavegameRepositoryTests
    {
        [SetUp]
        public void SetUp()
        {
            System.IO.File.WriteAllBytes(Binary, Savegame.RawData);
            _progress = new SavegameRepository(Binary).Load();
        }

        [TearDown]
        public void TearDown()
        {
            System.IO.File.Delete(Binary);
        }

        private static readonly File Binary = (File) "savegame.bin";
        private static Progress _progress;

        [Test]
        public void LoadSavegame_DifficultyIsCorrect_True()
        {
            Assert.AreEqual(Difficulty.Legendary, _progress.Difficulty);
        }

        [Test]
        public void LoadSavegame_MissionIsCorrect_True()
        {
            Assert.AreEqual(Mission.Spv3a30, _progress.Mission);
        }
    }
}