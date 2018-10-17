using System.Collections.Generic;
using System.IO;
using Atarashii.Exceptions;

namespace Atarashii.OpenSauce
{
    /// <summary>
    ///     Type for installing OpenSauce to the file system.
    /// </summary>
    public class Installer : IVerifiable
    {
        private readonly string _hcePath;

        private readonly List<Package> _packages;

        public Installer(string hcePath, List<Package> packages)
        {
            _hcePath = hcePath;
            _packages = packages;
        }

        /// <inheritdoc />
        public Verification Verify()
        {
            if (!Directory.Exists(_hcePath))
                return new Verification(false, "Target directory for OpenSauce installation does not exist.");

            if (!File.Exists(Path.Combine(_hcePath, Executable.Name)))
                return new Verification(false, "Invalid target HCE directory path for OpenSauce installation.");

            foreach (var package in _packages)
            {
                var packageState = package.Verify();
                if (!packageState.IsValid)
                    return new Verification(false, "An OpenSauce package does not exist on the filesystem.");
            }

            return new Verification(true);
        }

        /// <summary>
        ///     Installs the OpenSauce libraries to the given HCE directory path.
        /// </summary>
        /// <exception cref="OpenSauceException">
        ///     Invalid HCE directory path.
        ///     - or -
        ///     Target directory does not exist.
        ///     - or -
        ///     Package does not exist.
        /// </exception>
        public void Install()
        {
            var state = Verify();

            if (!state.IsValid)
                throw new OpenSauceException(state.Reason);

            foreach (var package in _packages)
                package.Install();
        }
    }
}