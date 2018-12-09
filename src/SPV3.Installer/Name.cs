using System;

namespace SPV3.Installer
{
    /// <summary>
    ///     Represents the identifier/filename for a package, file or directory.
    /// </summary>
    public class Name
    {
        private string _value;

        /// <summary>
        ///     The identifier/filename for a package, file or directory.
        /// </summary>
        /// <example>
        ///     0x01.pkg
        /// </example>
        /// <example>
        ///     maps
        /// </example>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Name value exceeds 255 characters.
        /// </exception>
        public string Value
        {
            get => _value;
            set
            {
                if (value.Length > 0xFF)
                    throw new ArgumentOutOfRangeException(nameof(value), "Name value exceeds 255 characters.");

                _value = value;
            }
        }
    }
}