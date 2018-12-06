using NUnit.Framework;

namespace SPV3.Loader.Test
{
    [TestFixture]
    public class ParameterTest
    {
        [Test]
        public void ParseType_TogglesAreParsed_True()
        {
            var parsed = new ParametersParser().Parse("-nosound -novideo -safemode -window");

            Assert.IsTrue(
                parsed.DisableSound
                && parsed.DisableVideo
                && parsed.EnableSafeMode
                && parsed.EnableWindowMode
            );
        }

        [Test]
        public void SerialiseType_CardTypeIsCorrect_True()
        {
            Assert.AreEqual("-use14", new ParametersSerialiser().Serialise(
                new Parameters
                {
                    CardType = CardType.Shaders14Card
                })
            );
        }

        [Test]
        public void SerialiseType_CorrectVideoMode_True()
        {
            Assert.AreEqual("-vidmode 1280,720,60", new ParametersSerialiser().Serialise(
                new Parameters
                {
                    VideoWidth = 1280,
                    VideoHeight = 720,
                    VideoRefreshRate = 60
                })
            );
        }

        [Test]
        public void SerialiseType_PortsAreCorrect_True()
        {
            Assert.AreEqual("-port 2302 -cport 2303", new ParametersSerialiser().Serialise(
                new Parameters
                {
                    ServerPort = 2302,
                    ClientPort = 2303
                })
            );
        }

        [Test]
        public void SerialiseType_TogglesAreCorrect_True()
        {
            Assert.AreEqual("-nosound -safemode -console", new ParametersSerialiser().Serialise(
                new Parameters
                {
                    DisableSound = true,
                    EnableSafeMode = true,
                    EnableConsole = true
                })
            );
        }
    }
}