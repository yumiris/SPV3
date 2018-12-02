using System;

namespace BalsamV.Settings
{
    /// <summary>
    ///     Representation of the audio volume settings.
    /// </summary>
    public class Volume
    {
        /// <summary>
        ///     Effects volume value.
        ///     This value is expected to be between 0 and 10.
        /// </summary>
        private byte _effects = 10;

        /// <summary>
        ///     Master volume value.
        ///     This value is expected to be between 0 and 10.
        /// </summary>
        private byte _master = 10;

        /// <summary>
        ///     Music volume value.
        ///     This value is expected to be between 0 and 10.
        /// </summary>
        private byte _music = 10;

        /// <summary>
        ///     Master volume value.
        ///     <exception cref="ArgumentOutOfRangeException">
        ///         Assigned volume value is greater than 10.
        ///     </exception>
        /// </summary>
        public byte Master
        {
            get => _master;
            set
            {
                if (!IsValid(value))
                    throw new ArgumentOutOfRangeException(nameof(value), "Assigned volume value is greater than 10.");

                _master = value;
            }
        }

        /// <summary>
        ///     Effects volume value.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Assigned volume value is greater than 10.
        /// </exception>
        public byte Effects
        {
            get => _effects;
            set
            {
                if (!IsValid(value))
                    throw new ArgumentOutOfRangeException(nameof(value), "Assigned volume value is greater than 10.");

                _effects = value;
            }
        }

        /// <summary>
        ///     Music volume value.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Assigned volume value is greater than 10.
        /// </exception>
        public byte Music
        {
            get => _music;
            set
            {
                if (!IsValid(value))
                    throw new ArgumentOutOfRangeException(nameof(value), "Assigned volume value is greater than 10.");

                _music = value;
            }
        }

        /// <summary>
        ///     Checks if the inbound volume value falls within a valid range.
        /// </summary>
        /// <param name="value">Inbound value to check.</param>
        /// <returns>True on valid value, otherwise false.</returns>
        private static bool IsValid(byte value)
        {
            return value < 11;
        }
    }
}