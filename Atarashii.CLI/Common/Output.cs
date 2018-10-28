using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Atarashii.CLI.Common
{
    /// <summary>
    ///     Abstract type representing console output data.
    /// </summary>
    public abstract class Output
    {
        /// <summary>
        ///     File information of the calling assembly.
        /// </summary>
        protected static readonly FileVersionInfo Exe =
            FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

        protected static class Git
        {
            private const string RevisionFile = "Atarashii.CLI.REVISION";

            public static string Revision
            {
                get
                {
                    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(RevisionFile))
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}