using NUnit.Framework;

namespace Atarashii.Tests
{
    [TestFixture]
    public class OpenSauceTests
    {
        private readonly string result = new OpenSauce().ToXml();

        [Test]
        public void ToXml_BooleansAreCorrect_True()
        {
            StringAssert.Contains("<DepthFade>true</DepthFade>", result);
        }

        [Test]
        public void ToXml_NumbersAreCorrect_True()
        {
            StringAssert.Contains("<FieldOfView>70</FieldOfView>", result);
        }
    }
}