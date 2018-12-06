using NUnit.Framework;

namespace SPV3.Loader.Test
{
    [TestFixture]
    public class ParameterTest
    {
        [Test]
        public void SerialiseType_CorrectVideoMode_True()
        {
            Assert.AreEqual("-vidmode 1280,720,60", new ExecutableParameters
            {
                VideoWidth = 1280,
                VideoHeight = 720,
                VideoRefreshRate = 60
            }.Serialise());
        }

        [Test]
        public void SerialiseType_TogglesAreCorrect_True()
        {
            Assert.AreEqual("-nosound -nojoystick -safemode -console", new ExecutableParameters
            {
                DisableSound = true,
                DisableJoystick = true,
                
                EnableSafeMode = true,
                EnableConsole = true
            }.Serialise());
        }

        [Test]
        public void SerialiseType_CardTypeIsCorrect_True()
        {
            Assert.AreEqual("-use14", new ExecutableParameters
            {
                CardType = CardType.Shaders14Card
            }.Serialise());
        }

        [Test]
        public void SerialiseType_PortsAreCorrect_True()
        {
            Assert.AreEqual("-port 2302 -cport 2303", new ExecutableParameters
            {
                ServerPort = 2302,
                ClientPort = 2303
            }.Serialise());
        }
    }
}