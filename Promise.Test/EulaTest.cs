using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Promise.Library;

namespace Promise.Test
{
    [TestClass]
    public class EulaTest
    {
        [TestMethod]
        public void WriteEulaLibrary_LibraryIsDumped_True()
        {
            Eula eula = new Eula();
            eula.WriteEulaLibrary();

            Assert.AreEqual(File.Exists(Eula.EulaLibrary), true);
        }

        [TestMethod]
        public void WriteEulaDocument_DocumentIsDumped_True()
        {
            Eula eula = new Eula();
            eula.WriteEulaDocument();

            Assert.AreEqual(File.Exists(Eula.EulaDocument), true);
        }
    }
}