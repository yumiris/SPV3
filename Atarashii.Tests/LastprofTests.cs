using NUnit.Framework;

namespace Atarashii.Tests
{
    [TestFixture]
    public class LastprofTests
    {
        [Test]
        public void ParseTest_CorrectProfileName_True()
        {
            var result = new Profile.Lastprof().Parse(@"E:\roman\Documents\My Games\Halo CE\savegames\Miris\ lam.sav           ");
            Assert.That(result, Is.EqualTo("Miris"));
        }
    }
}