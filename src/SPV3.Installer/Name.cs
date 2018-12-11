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
        ///     Name value exceeds 64 characters.
        /// </exception>
        public string Value
        {
            get => _value;
            set
            {
                if (value.Length > 0x40)
                    throw new ArgumentOutOfRangeException(nameof(value), $"Name '{value}' exceeds 64 characters.");

                _value = value;
            }
        }

        /// <summary>
        ///     Implicitly represents object as string.
        /// </summary>
        /// <param name="name">
        ///     Object instance.
        /// </param>
        /// <returns>
        ///     Name.Value
        /// </returns>
        public static implicit operator string(Name name)
        {
            return name.Value;
        }

        /// <summary>
        ///     Explicitly represents string as object.
        /// </summary>
        /// <param name="value">
        ///     Name.Value
        /// </param>
        /// <returns>
        ///     Object instance.
        /// </returns>
        public static explicit operator Name(string value)
        {
            return new Name
            {
                Value = value
            };
        }
    }
}