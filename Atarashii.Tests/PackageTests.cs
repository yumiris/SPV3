using System;
using NUnit.Framework;

namespace Atarashii.Tests
{
    [TestFixture]
    public class PackageTests
    {
        [Test]
        public void PackageNotFound_ThrowsException()
        {
            var package = new Package(new Guid().ToString(), "Non-existent package!", string.Empty);
            
            var ex = Assert.Throws<PackageException>(() => package.Install());
            Assert.That(ex.Message, Is.EqualTo("Cannot install specified package. Archive does not exist."));
        }
    }
}