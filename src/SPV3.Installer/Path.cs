using System;

namespace SPV3.Installer
{
    /// <summary>
    ///     Represents the absolute path of a directory or file.
    /// </summary>
    public class Path
    {
        private string _value;

        /// <summary>
        ///     The absolute path of a directory or file.
        /// </summary>
        /// <example>
        ///     C:\SPV3.2\maps
        /// </example>
        /// <example>
        ///     C:\SPV3.2\maps\loc.map
        /// </example>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Path value exceeds 255 characters.
        /// </exception>
        public string Value
        {
            get => _value;
            set
            {
                if (value.Length > 0xFF)
                    throw new ArgumentOutOfRangeException(nameof(value), "Path value exceeds 255 characters.");

                _value = value;
            }
        }
    }
}