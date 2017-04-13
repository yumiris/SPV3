using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Promise.Library;

namespace Promise.Test
{
    [TestClass]
    public class EulaInjectionTest
    {
        [TestMethod]
        public void WriteEulaLibrary_LibraryIsDumped_True()
        {
            var eulaInjection = new EulaInjection();
            eulaInjection.WriteEulaLibrary();
            Assert.AreEqual(File.Exists("eula.dll"), true);
        }

        [TestMethod]
        public void WriteEulaDocument_DocumentIsDumped_True()
        {
            var eulaInjection = new EulaInjection();
            eulaInjection.WriteEulaDocument();
            Assert.AreEqual(File.Exists("eula.rtf"), true);
        }
    }
}