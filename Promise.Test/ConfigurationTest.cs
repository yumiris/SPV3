using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Promise.Library.Utilities;

namespace Promise.Test
{
    [TestClass]
    public class ConfigurationTest
    {
        private readonly ConfigOperation _configOperation = new ConfigOperation();
        private readonly string testConfigString = "-window";

        [TestMethod]
        public void WriteConfiguration_ConfigurationWritten_True()
        {
            File.Delete(ConfigOperation.FileName);
            _configOperation.WriteConfiguration(testConfigString);
            Assert.AreEqual(File.Exists(ConfigOperation.FileName), true);
        }

        [TestMethod]
        public void ReadConfiguration_ContainsParameter_True()
        {
            var configurationData = _configOperation.ReadConfiguration();
            Assert.AreEqual(configurationData.Contains(testConfigString), true);
        }
    }
}