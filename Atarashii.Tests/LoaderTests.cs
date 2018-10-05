using System;
using System.IO;
using Atarashii.Executable;
using NUnit.Framework;

namespace Atarashii.Tests
{
    [TestFixture]
    public class LoaderTests
    {
        [Test]
        public void LoadInvalidExecutable_ThrowsException_True()
        {
            var executable = $"{new Guid().ToString()}.exe";
            var loader = new Loader();
            var verifier = new Verifier();

            File.WriteAllText(executable, "Once upon a time, in Gensokyo...");

            var ex = Assert.Throws<LoaderException>(() => loader.Execute(executable, verifier));
            Assert.That(ex.Message, Is.EqualTo($"The specified executable '{executable}' is deemed invalid."));

            File.Delete(executable);
        }

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