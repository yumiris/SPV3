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
            _savegame = new SavegameRepository(Binary).Load();
        }

        [TearDown]
        public void TearDown()
        {
            System.IO.File.Delete(Binary);
        }

        private static readonly File Binary = (File) "savegame.bin";
        private static Resume.Savegame _savegame;

        [Test]
        public void LoadSavegame_DifficultyIsCorrect_True()
        {
            Assert.AreEqual(Difficulty.Legendary, _savegame.Difficulty);
        }

        [Test]
        public void LoadSavegame_MissionIsCorrect_True()
        {
            Assert.AreEqual(Mission.Spv3a30, _savegame.Mission);
        }
    }
}