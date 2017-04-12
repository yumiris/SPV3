using System.IO;
using Promise.Library.Properties;

namespace Promise.Library
{
    public class Eula
    {
        public const string EulaLibrary = "eula.dll";
        public const string EulaDocument = "eula.rtf";

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