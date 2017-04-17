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
            if (!File.Exists(EulaLibrary))
            {
                File.WriteAllBytes(EulaLibrary, Resources.EulaLib);
            }
        }

        public void WriteEulaDocument()
        {
            if (!File.Exists(EulaDocument))
            {
                File.WriteAllText(EulaDocument, Resources.EulaDoc);
            }
        }
    }
}