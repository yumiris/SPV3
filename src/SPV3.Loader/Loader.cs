using System;
using System.Diagnostics;

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
        ///     Executable-type instance representing the HCE executable to load.
        /// </summary>
        private readonly Executable _executable;

        /// <summary>
        ///     Loader constructor.
        /// </summary>
        /// <param name="executable">
        ///     <see cref="_executable" />
        /// </param>
        /// <param name="configuration">
        ///     <see cref="_configuration" />
        /// </param>
        public Loader(Executable executable, LoaderConfiguration configuration)
        {
            _executable = executable;
            _configuration = configuration;
        }

        /// <summary>
        ///     Starts up the inbound executable with the loader configuration specified in the constructor.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Provided executable failed to pass the verification routine.
        /// </exception>
        public void Start()
        {
            if (!_configuration.SkipVerification)
                if (!_executable.Verify())
                    throw new ArgumentException("Provided executable failed to pass the verification routine.");

            Process.Start(_executable.Path);
        }
    }
}