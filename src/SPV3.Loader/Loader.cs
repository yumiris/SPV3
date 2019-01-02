/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Loader.
 * 
 * SPV3.Loader is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Loader is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Loader.  If not, see <http://www.gnu.org/licenses/>.
 */

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
        ///     Optional parameters used for initialising the HCE executable process.
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
        public void Start(Executable executable, Parameters parameters = null)
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

                    // optional startup args
                    Arguments = parameters == null
                        ? string.Empty
                        : new ParametersSerialiser().Serialise(parameters)
                }
            }.Start();
        }
    }
}