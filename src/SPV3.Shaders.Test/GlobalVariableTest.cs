using System;
using NUnit.Framework;
using SPV3.Shaders.Options;

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

        [Test]
        public void AssertProperty_IsEncodedCorrectly_True()
        {
            var configuration = new Configuration
            {
                AmbientOcclusion = {Level = Level.High},
                DepthOfField = {Level = Level.Low},
                DynamicFlare = {Toggle = Toggle.On},
                LensDirt = {Toggle = Toggle.On},
                EyeAdaption = {Toggle = Toggle.Off},
                AntiAliasing = {Toggle = Toggle.On},
                Debanding = {Level = Level.Low}
            };
            
            Assert.AreEqual(42644, GlobalVariableFactory.Encode(configuration).Value);
        }
    }
}