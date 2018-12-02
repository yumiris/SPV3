using System;

namespace SPV3.Shaders
{
    public class ShaderState
    {
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
            if
            (
                (value & (value - 1)) != 0 // power of two
                || value < 0x1 || value > 0x10000 // bounds
            )
                throw new ArgumentException("Given value does not represent a valid shader state.");

            Value = value;
        }

        /// <summary>
        ///     Integer representation for a shader state.
        ///     The value is expected to represent a valid shader state, as specified in the doc/global-variable.md
        ///     document.
        /// </summary>
        public int Value { get; }
    }
}