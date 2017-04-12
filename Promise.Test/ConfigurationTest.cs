using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Promise.Library;

namespace Promise.Test
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void WriteConfiguration_ConfigurationWritten_True()
        {
            Configuration configuration = new Configuration
            {
                IsWindow = true,
                IsConsole = true,
                IsDev = true,
                IsSafeMode = true
            };

            configuration.WriteConfiguration();

            Assert.AreEqual(File.Exists("config.ini"), true);
        }
    }
}
