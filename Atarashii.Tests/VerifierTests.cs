using System;
using System.IO;
using Atarashii.Executable;
using NUnit.Framework;

namespace Atarashii.Tests
{
    [TestFixture]
    public class VerifierTests
    {
        [Test]
        public void VerifyValidExecutable_ValueIsTrue_True()
        {
            var executable = $"{new Guid().ToString()}.exe";
            var verifier = new Verifier();

            File.WriteAllBytes(executable, new byte[0x24B000]);
            Assert.IsTrue(verifier.Verify(executable));
            File.Delete(executable);
        }
    }
}