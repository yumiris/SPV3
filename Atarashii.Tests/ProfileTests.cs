using System;
using Atarashii.Modules.Profile;
using NUnit.Framework;

namespace Atarashii.Tests
{
    [TestFixture]
    public class ProfileTests
    {
        [Test]
        public void InvalidResolution_ThrowsException_True()
        {
            var configuration = new Configuration();

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => configuration.Video.Resolution.Width = 0);
            StringAssert.Contains("Assigned dimension value is either 0 or over 32767.", ex.Message);
        }

        [Test]
        public void InvalidVolume_ThrowsException_True()
        {
            var configuration = new Configuration();

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => configuration.Audio.Volume.Music = 15);
            StringAssert.Contains("Assigned volume value is greater than 10.", ex.Message);
        }

        [Test]
        public void InvalidName_ThrowsException_True()
        {
            var configuration = new Configuration();

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => configuration.Name.Value = "Hello from Gensokyo");
            StringAssert.Contains("Assigned name value is greater than 11 characters.", ex.Message);
        }

        [Test]
        public void InvalidSensitivity_ThrowsException_True()
        {
            var configuration = new Configuration();

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => configuration.Mouse.Sensitivity.Horizontal = 11);
            StringAssert.Contains("Assigned sensitivity value is less than 1 or greater than 10.", ex.Message);
        }
    }
}