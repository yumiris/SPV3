using System;

namespace SPV3.Installer
{
    /// <summary>
    ///     Identifier for a package, file or directory.
    /// </summary>
    public class Name
    {
        private string _value;

        /// <example>
        ///     0x01.pkg
        /// </example>
        /// <example>
        ///     maps
        /// </example>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Name value exceeds 32 characters.
        /// </exception>
        public string Value
        {
            get => _value;
            set
            {
                if (value.Length > 0x20)
                    throw new ArgumentOutOfRangeException(nameof(value), "Name value exceeds 32 characters.");

                _value = value;
            }
        }
    }
}