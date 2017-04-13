using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Promise.Library;
using Promise.Library.Configuration;

namespace Promise.Test
{
    [TestClass]
    public class ConfigurationTest
    {
        private readonly HaloConfiguration _haloConfiguration = new HaloConfiguration();

        [TestMethod]
        public void WriteConfiguration_ConfigurationWritten_True()
        {
            File.Delete("config.ini");
            _haloConfiguration.WriteConfiguration("-window");
            Assert.AreEqual(File.Exists("config.ini"), true);
        }

        [TestMethod]
        public void ReadConfiguration_ContainsParameter_True()
        {
            string configurationData = _haloConfiguration.ReadConfiguration();
            Assert.AreEqual(configurationData.Contains("-window"), true);
        }
    }
}