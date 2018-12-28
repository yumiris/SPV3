using NUnit.Framework;

namespace SPV3.Domain.Tests
{
    [TestFixture]
    public class VersionTests
    {
        [Test]
        public void ImplicitCreation_VersionIsCorrect()
        {
            var version = new Version
            {
                Major = 2,
                Minor = 4,
                Patch = 8
            };

            Assert.AreEqual("2.4.8", (string) version);
        }

        [Test]
        public void ExplicitCreation_VersionIsCorrect()
        {
            var version = (Version) "2.4.8";

            Assert.AreEqual("2.4.8", (string) version);
        }
    }
}