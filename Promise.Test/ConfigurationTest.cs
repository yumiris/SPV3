using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Promise.Library.Configuration;

namespace Promise.Test
{
    [TestClass]
    public class ConfigurationTest
    {
        private readonly HaloConfiguration _haloConfiguration = new HaloConfiguration();
        private string testConfigString = "-window";

        [TestMethod]
        public void WriteConfiguration_ConfigurationWritten_True()
        {
            File.Delete(HaloConfiguration.ConfigurationFile);
            _haloConfiguration.WriteConfiguration(testConfigString);
            Assert.AreEqual(File.Exists(HaloConfiguration.ConfigurationFile), true);
        }

        [TestMethod]
        public void ReadConfiguration_ContainsParameter_True()
        {
            var configurationData = _haloConfiguration.ReadConfiguration();
            Assert.AreEqual(configurationData.Contains(testConfigString), true);
        }
    }
}