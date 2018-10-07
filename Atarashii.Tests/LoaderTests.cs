using System;
using System.IO;
using Atarashii.Exceptions;
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
            var executable = new Loader(exeName);

            File.WriteAllText(exeName, "Once upon a time, in Gensokyo...");

            var ex = Assert.Throws<LoaderException>(() => executable.Execute());
            Assert.That(ex.Message, Is.EqualTo($"The specified executable '{exeName}' is deemed invalid."));

            File.Delete(exeName);
        }

        [Test]
        public void LoadNonExistentExecutable_ThrowsException_True()
        {
            var exeName = $"{new Guid().ToString()}.exe";
            var executable = new Loader(exeName);

            var ex = Assert.Throws<LoaderException>(() => executable.Execute());
            Assert.That(ex.Message, Is.EqualTo($"The specified executable '{exeName}' was not found."));
        }
    }
}