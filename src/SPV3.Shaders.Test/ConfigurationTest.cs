using NUnit.Framework;
using SPV3.Shaders.Options;

namespace SPV3.Shaders.Test
{
    [TestFixture]
    public class ConfigurationTest
    {
        /// <summary>
        ///     Asserts that when <see cref="ConfigurationFactory" /> is provided with a number that equates to an
        ///     encoded user configuration, the factory returns a Configuration-type instance with the correct
        ///     property values.
        /// </summary>
        [Test]
        public void Configuration_IsDecodedCorrectly_True()
        {
            var config = ConfigurationFactory.Decode(new GlobalVariable(42132));

            Assert.AreEqual(Level.High, config.AmbientOcclusion.Level);
            Assert.AreEqual(Level.Low, config.DepthOfField.Level);
            Assert.AreEqual(Toggle.On, config.DynamicFlare.Toggle);
            Assert.AreEqual(Toggle.Off, config.EyeAdaption.Toggle);
            Assert.AreEqual(Toggle.On, config.AntiAliasing.Toggle);
            Assert.AreEqual(Level.Low, config.Debanding.Level);
        }
    }
}