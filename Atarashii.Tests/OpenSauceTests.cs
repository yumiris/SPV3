using System;
using System.IO;
using Atarashii.Exceptions;
using NUnit.Framework;

namespace Atarashii.Tests
{
    [TestFixture]
    public class OpenSauceTests
    {
        private readonly string result = new OpenSauce().ToXml();

        [Test]
        public void FromXml_PropertyValueIsCorrect_True()
        {
            var object01 = new OpenSauce
            {
                Camera = new Camera
                {
                    FieldOfView = 128.64
                }
            };

            var object02 = OpenSauceFactory.GetFromXml(object01.ToXml());
            Assert.AreEqual(object02.Camera.FieldOfView, object01.Camera.FieldOfView);
        }

        [Test]
        public void InstallToInvalidHceDirectory_ThrowsException()
        {
            var opensauce = new OpenSauce();
            string hcePath = new Guid().ToString();

            Directory.CreateDirectory(hcePath);

            var ex = Assert.Throws<OpenSauceException>(() => opensauce.InstallTo(hcePath, new MockLogger()));
            Assert.That(ex.Message, Is.EqualTo("Cannot install OpenSauce. Invalid target HCE directory path."));

            Directory.Delete(hcePath);
        }

        [Test]
        public void InstallToNonExistentDirectory_ThrowsException()
        {
            var opensauce = new OpenSauce();
            string falsePath = new Guid().ToString();

            var ex = Assert.Throws<OpenSauceException>(() => opensauce.InstallTo(falsePath, new MockLogger()));
            Assert.That(ex.Message,
                Is.EqualTo("Cannot install OpenSauce. Installation target directory does not exist."));
        }

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