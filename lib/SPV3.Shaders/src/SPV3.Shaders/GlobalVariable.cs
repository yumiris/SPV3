using System;

namespace SPV3.Shaders
{
    /// <summary>
    ///     Type representing the value that represents the Global Variable value. The Global Variable stores the
    ///     encoded representation of a Configuration type, as specified in the doc/global-variable.md documentation.
    /// </summary>
    public class GlobalVariable
    {
        /// <summary>
        ///     Lowest valid bound, which is the sum of all of the shader effects' lowest valued states.
        ///     Please refer to the doc/global-variable.md documentation for further information.
        /// </summary>
        private const int LowerBound = 21833;

        /// <summary>
        ///     Lowest valid bound, which is the sum of all of the shader effects' highest valued states.
        ///     Please refer to the doc/global-variable.md documentation for further information.
        /// </summary>
        private const int UpperBound = 76452;

        /// <summary>
        ///     Encoded Configuration value. Please refer to the doc/global-variable.md documentation for information.
        /// </summary>
        public int Value { get; }

        /// <summary>
        ///     Representation constructor.
        /// </summary>
        /// <param name="value">
        ///     <see cref="Value" />
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Given value is out of bounds for possible configurations.
        /// </exception>
        public GlobalVariable(int value)
        {
            if (value < LowerBound || value > UpperBound)
                throw new ArgumentException("Given value is out of bounds for the global variable.");

            Value = value;
        }
    }
}