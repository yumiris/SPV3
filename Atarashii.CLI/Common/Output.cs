using System.Diagnostics;
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
        protected static readonly FileVersionInfo Info =
            FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
    }
}