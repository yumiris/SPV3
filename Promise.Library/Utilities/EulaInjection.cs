using System.IO;
using Promise.Library.Properties;

namespace Promise.Library.Utilities
{
    public class EulaInjection
    {
        private const string EulaLibrary = "eula.dll";
        private const string EulaDocument = "eula.rtf";

        public void WriteEulaLibrary()
        {
            File.WriteAllBytes(EulaLibrary, Resources.EulaLib);
        }

        public void WriteEulaDocument()
        {
            File.WriteAllText(EulaDocument, Resources.EulaDoc);
        }
    }
}