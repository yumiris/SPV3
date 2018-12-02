using System;

namespace BalsamV
{
    /// <summary>
    ///     This type is used to represent a lastprof.txt text file.
    /// </summary>
    public class Lastprof
    {
        /// <summary>
        ///     Separation character which is guaranteed to be present.
        /// </summary>
        public const char Delimiter = '\\';

        /// <summary>
        ///     Position of the profile name relative to the end of the split string.
        /// </summary>
        public const int NameOffset = 0x2;

        /// <summary>
        ///     Known string that is present in the lastprof.txt.
        /// </summary>
        public const string Signature = "lam.sav";

        /// <summary>
        ///     Name of the last accessed profile.
        ///     This value is expected to be between 1 and 11 characters.
        /// </summary>
        private string _name;

        /// <summary>
        ///     <see cref="_name" />
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(value);

                if (value.Length > 0xB)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "Lastprof.txt name value is greater than 11 characters.");

                _name = value;
            }
        }
    }
}