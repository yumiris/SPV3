using System.Collections.Generic;
using System.IO;
using Atarashii.Exceptions;

namespace Atarashii
{
    /// <summary>
    ///     Type for installing OpenSauce to the file system.
    /// </summary>
    public class OpenSauceInstaller : IVerifiable
    {
        private readonly string _hcePath;

        private readonly ILogger _logger;

        private readonly List<Package> _packages;

        public OpenSauceInstaller(string hcePath, List<Package> packages)
        {
            _hcePath = hcePath;
            _packages = packages;
        }

        public OpenSauceInstaller(string hcePath, List<Package> packages, ILogger logger) : this(hcePath, packages)
        {
            _logger = logger;
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
                package.Install(_logger);
        }
    }
}