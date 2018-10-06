using System;
using System.IO;
using NUnit.Framework;

namespace Atarashii.Tests
{
    [TestFixture]
    public class LoaderTests
    {
        [Test]
        public void LoadInvalidExecutable_ThrowsException_True()
        {
            var exeName = $"{new Guid().ToString()}.exe";
            var executable = new Executable();

            File.WriteAllText(exeName, "Once upon a time, in Gensokyo...");

            var ex = Assert.Throws<LoaderException>(() => executable.Load(exeName));
            Assert.That(ex.Message, Is.EqualTo($"The specified executable '{exeName}' is deemed invalid."));

            File.Delete(exeName);
        }

        [Test]
        public void LoadNonExistentExecutable_ThrowsException_True()
        {
            var exeName = $"{new Guid().ToString()}.exe";
            var executable = new Executable();

            var ex = Assert.Throws<LoaderException>(() => executable.Load(exeName));
            Assert.That(ex.Message, Is.EqualTo($"The specified executable '{exeName}' was not found."));
        }
    }
}