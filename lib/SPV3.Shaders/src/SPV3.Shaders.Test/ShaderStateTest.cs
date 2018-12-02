using System;
using NUnit.Framework;

namespace SPV3.Shaders.Test
{
    [TestFixture]
    public class ShaderStateTest
    {
        [Test]
        public void AssertException_InvalidPowerOfTwoNumberIsAssigned_True()
        {
            Assert.Throws<ArgumentException>(() => new ShaderState(13));
        }

        [Test]
        public void AssertException_LargeNumberIsAssigned_True()
        {
            Assert.Throws<ArgumentException>(() => new ShaderState(48212887));
        }

        [Test]
        public void AssertException_SmallNumberIsAssigned_True()
        {
            Assert.Throws<ArgumentException>(() => new ShaderState(0));
        }
    }
}