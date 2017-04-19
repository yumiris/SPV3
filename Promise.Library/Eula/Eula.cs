using System.IO;

namespace Promise.Library.Eula
{
    public class Eula
    {
        private const string EulaLibrary = "eula.dll";

        public void Inject()
        {
            if (File.Exists(EulaLibrary)) return;
            try
            {
                File.WriteAllBytes(EulaLibrary, EulaResource.EulaDLL);
            }
            catch (IOException)
            {
                throw new IOException("Could not dump the EULA. Administrator privileges may be required.");
            }
        }
    }
}