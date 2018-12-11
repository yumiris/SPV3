using System;

namespace SPV3.Installer
{
    /// <summary>
    ///     Represents an user-friendly description of an entity (e.g. File or Package).
    /// </summary>
    public class Description
    {
        private string _value;

        /// <example>
        ///     SPV3.2 Maps for HCE
        /// </example>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Description value exceeds 128 characters.
        /// </exception>
        public string Value
        {
            get => _value;
            set
            {
                if (value.Length > 0x80)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Description '{value}' exceeds 128 characters.");

                _value = value;
            }
        }

        /// <summary>
        ///     Implicitly represents object as string.
        /// </summary>
        /// <param name="description">
        ///     Object instance.
        /// </param>
        /// <returns>
        ///     Description.Value
        /// </returns>
        public static implicit operator string(Description description)
        {
            return description.Value;
        }

        /// <summary>
        ///     Explicitly represents string as object.
        /// </summary>
        /// <param name="value">
        ///     Description.Value
        /// </param>
        /// <returns>
        ///     Object instance.
        /// </returns>
        public static explicit operator Description(string value)
        {
            return new Description
            {
                Value = value
            };
        }
    }
}