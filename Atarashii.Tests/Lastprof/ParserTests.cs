using Atarashii.Profile.Lastprof;
using NUnit.Framework;

namespace Atarashii.Tests.Lastprof
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void ParseTest_CorrectProfileName_True()
        {
            var result = new Parser().Parse(@"E:\roman\Documents\My Games\Halo CE\savegames\Miris\ lam.sav           ");
            Assert.That(result, Is.EqualTo("Miris"));
        }
    }
}