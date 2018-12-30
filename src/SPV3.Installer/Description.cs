using System;

namespace SPV3.Installer
{
    /// <summary>
    ///     Human-readable description for a complex type.
    /// </summary>
    public class Description
    {
        /// <summary>
        ///     Maximum length allowed for the provided value.
        /// </summary>
        public const int MaxLength = 0xFF;

        /// <summary>
        ///     <see cref="Value" />
        /// </summary>
        private string _value;

        /// <summary>
        ///     Description value.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Value length exceeds upper bound. <see cref="MaxLength" />
        /// </exception>
        public string Value
        {
            get => _value;
            set
            {
                if (value.Length > MaxLength)
                {
                    var message = $"Value '{value}' length exceeds upper bound of {MaxLength}.";
                    throw new ArgumentOutOfRangeException(nameof(value), message);
                }

                _value = value;
            }
        }

        /// <summary>
        ///     Represent object as string.
        /// </summary>
        /// <param name="description">
        ///     Object to represent as string.
        /// </param>
        /// <returns>
        ///     String representation of the object.
        /// </returns>
        public static implicit operator string(Description description)
        {
            return description.Value;
        }

        /// <summary>
        ///     Represent string as object.
        /// </summary>
        /// <param name="value">
        ///     String to represent as object.
        /// </param>
        /// <returns>
        ///     Object representation of the string.
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