using System;
using NUnit.Framework;

namespace Atarashii.Tests
{
    [TestFixture]
    public class LoaderTests
    {
        [Test]
        public void LoadNonExistentExecutable_ThrowsException_True()
        {
            var executable = $"{new Guid().ToString()}.exe";
            var loader = new Loader();

            var ex = Assert.Throws<LoaderException>(() => loader.Execute(executable));
            Assert.That(ex.Message, Is.EqualTo($"The specified executable '{executable}' was not found."));
        }
    }
}