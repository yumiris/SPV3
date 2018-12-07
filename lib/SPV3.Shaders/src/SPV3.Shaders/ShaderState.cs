using System;

namespace SPV3.Shaders
{
    public class ShaderState
    {
        /// <summary>
        ///     Lowest valid bound, which is the lowest shader state integer representation as specified in the
        ///     doc/global-variable.md documentation.
        /// </summary>
        private const int LowerBound = 0x00001;

        /// <summary>
        ///     Highest valid bound, which is the highest shader state integer representation as specified in the
        ///     doc/global-variable.md documentation.
        /// </summary>
        private const int UpperBound = 0x4000;

        /// <summary>
        ///     Integer representation for a shader state.
        ///     The value is expected to represent a valid shader state, as specified in the doc/global-variable.md
        ///     document.
        /// </summary>
        public int Value { get; }

        /// <summary>
        ///     State constructor.
        /// </summary>
        /// <param name="value">
        ///     <see cref="Value" />
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Given value does not represent a valid shader state.
        /// </exception>
        public ShaderState(int value)
        {
            // power of two & bound verification
            if ((value & (value - 1)) != 0 || value < LowerBound || value > UpperBound)
                throw new ArgumentException("Given value does not represent a valid shader state.");

            Value = value;
        }
    }
}