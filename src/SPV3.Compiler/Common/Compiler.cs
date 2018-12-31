using SPV3.Domain;
using SPV3.Installer;

namespace SPV3.Compiler.Common
{
    public abstract class Compiler
    {
        /// <summary>
        ///     Prefix used for the package/manifest files.
        /// </summary>
        protected const string Prefix = "0x";

        /// <summary>
        ///     Suffix (or extension) used for the package/manifest files.
        /// </summary>
        protected const string Suffix = ".bin";

        protected readonly Compressor _compressor;
        protected readonly IStatus _status;

        protected Compiler(Compressor compressor, IStatus status = null)
        {
            _compressor = compressor;
            _status = status;
        }

        public abstract Manifest Compile(Directory source, Directory target);

        /// <summary>
        ///     Wrapper for IStatus .CommitStatus().
        /// </summary>
        /// <param name="text">
        ///     Text to commit to the IStatus instance.
        /// </param>
        protected void Notify(string text)
        {
            _status?.CommitStatus(text);
        }
    }
}