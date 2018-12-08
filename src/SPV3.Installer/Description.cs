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
                    throw new ArgumentOutOfRangeException(nameof(value), "Description value exceeds 128 characters.");

                _value = value;
            }
        }
    }
}