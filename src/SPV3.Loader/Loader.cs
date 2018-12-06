using System;
using System.Diagnostics;
using System.IO;

namespace SPV3.Loader
{
    /// <summary>
    ///     Starts up the inbound executable with the inbound loader configuration.
    /// </summary>
    public class Loader
    {
        /// <summary>
        ///     Configuration used for the executable loading routine.
        /// </summary>
        private readonly LoaderConfiguration _configuration;

        /// <summary>
        ///     Loader constructor.
        /// </summary>
        /// <param name="configuration">
        ///     <see cref="_configuration" />
        /// </param>
        public Loader(LoaderConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        ///     Starts up the inbound executable with the loader configuration specified in the constructor.
        /// </summary>
        /// <param name="executable">
        ///     Executable-type instance representing the HCE executable to load.
        /// </param>
        /// <param name="parameters">
        ///     Parameters used for initialising the HCE executable process.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Provided executable failed to pass the verification routine.
        /// </exception>
        /// <exception cref="FormatException">
        ///     Could not infer executable name from the path.
        /// </exception>
        /// <exception cref="FormatException">
        ///     Could not infer working directory from the path.
        /// </exception>
        public void Start(Executable executable, Parameters parameters)
        {
            if (!_configuration.SkipVerification)
                if (!executable.Verify())
                    throw new ArgumentException("Provided executable failed to pass the verification routine.");

            new Process
            {
                StartInfo =
                {
                    FileName = Path.GetFileName(executable.Path) ??
                               throw new FormatException("Could not infer executable name from the path."),
                    WorkingDirectory = Path.GetDirectoryName(executable.Path) ??
                                       throw new FormatException("Could not infer working directory from the path."),
                    Arguments = new ParametersSerialiser().Serialise(parameters)
                }
            }.Start();
        }
    }
}