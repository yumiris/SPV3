using System;
using NUnit.Framework;

namespace SPV3.Shaders.Test
{
    [TestFixture]
    public class GlobalVariableTest
    {
        [Test]
        public void AssertException_LargeNumberIsAssigned_True()
        {
            Assert.Throws<ArgumentException>(() => new GlobalVariable(48212887));
        }

        [Test]
        public void AssertException_SmallNumberIsAssigned_True()
        {
            Assert.Throws<ArgumentException>(() => new GlobalVariable(0));
        }
    }
}