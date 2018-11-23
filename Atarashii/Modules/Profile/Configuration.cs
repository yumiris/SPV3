using System;
using Atarashii.Modules.Profile.Options;

namespace Atarashii.Modules.Profile
{
    // TODO: Controls/gamepad settings.
    // TODO: Documentation for the rest of the enum values.

    /// <summary>
    ///     This type is used to represent a HCE profile configuration.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        ///     Length of the blam.sav binary.
        /// </summary>
        public const int BlamLength = 0x2000;

        /// <summary>
        ///     Offset of the profile name property.
        /// </summary>
        public const int NameOffset = 0x2;

        /// <summary>
        ///     Data length of the profile name property.
        /// </summary>
        public const int NameLength = 0xB;

        /// <summary>
        ///     Offset of the player colour property.
        /// </summary>
        public const int ColourOffset = 0x11a;

        /// <summary>
        ///     Offset of the horizontal mouse sensitivity property.
        /// </summary>
        public const int MouseSensitivityHorizontalOffset = 0x954;

        /// <summary>
        ///     Offset of the vertical mouse sensitivity property.
        /// </summary>
        public const int MouseSensitivityVerticalOffset = 0x955;

        /// <summary>
        ///     Offset of the mouse vertical axis inversion property.
        /// </summary>
        public const int MouseInvertVerticalAxisOffset = 0x12F;

        /// <summary>
        ///     Offset of the audio master volume property.
        /// </summary>
        public const int AudioVolumeMasterOffset = 0xB78;

        /// <summary>
        ///     Offset of the audio master volume property.
        /// </summary>
        public const int AudioVolumeEffectsOffset = 0xB79;

        /// <summary>
        ///     Offset of the audio master volume property.
        /// </summary>
        public const int AudioVolumeMusicOffset = 0xB7A;

        /// <summary>
        ///     Offset of the audio quality property.
        /// </summary>
        public const int AudioQualityOffset = 0xB7D;

        /// <summary>
        ///     Offset of the audio variety property.
        /// </summary>
        public const int AudioVarietyOffset = 0xB7F;

        /// <summary>
        ///     Offset of the video resolution width property.
        /// </summary>
        public const int VideoResolutionWidthOffset = 0xA68;

        /// <summary>
        ///     Offset of the video resolution height property.
        /// </summary>
        public const int VideoResolutionHeightOffset = 0xA6A;

        /// <summary>
        ///     Offset of the video frame rate property.
        /// </summary>
        public const int VideoFrameRateOffset = 0xA6F;

        /// <summary>
        ///     Offset of the video specular effects property.
        /// </summary>
        public const int VideoEffectsSpecularOffset = 0xA70;

        /// <summary>
        ///     Offset of the video shadows effects property.
        /// </summary>
        public const int VideoEffectsShadowsOffset = 0xA71;

        /// <summary>
        ///     Offset of the video decals effects property.
        /// </summary>
        public const int VideoEffectsDecalsOffset = 0xA72;

        /// <summary>
        ///     Offset of the video particles property.
        /// </summary>
        public const int VideoParticlesOffset = 0xA73;

        /// <summary>
        ///     Offset of the video quality property.
        /// </summary>
        public const int VideoQualityOffset = 0xA74;

        /// <summary>
        ///     Offset of the network connection type property.
        /// </summary>
        public const int NetworkConnectionTypeOffset = 0xFC0;

        /// <summary>
        ///     Offset of the network server port property.
        /// </summary>
        public const int NetworkPortServerOffset = 0x1002;

        /// <summary>
        ///     Offset of the network client port property.
        /// </summary>
        public const int NetworkPortClientOffset = 0x1004;

        /// <summary>
        ///     Offset of the blam.sav integrity hash.
        /// </summary>
        public const int BlamHashOffset = 0x1FFC;

        /// <summary>
        ///     Length of the blam.sav integrity hash.
        ///     The value is that of a CRC32 length.
        /// </summary>
        public const int BlamHashLength = 0x4;

        /// <summary>
        ///     Player name.
        ///     This value is expected to be between 1 and 11 characters.
        /// </summary>
        private string _name = "New001";

        /// <summary>
        ///     Player name value.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     No name value has been been assigned.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Assigned name value is greater than 11 characters.
        /// </exception>
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(value);

                if (value.Length > 0xB)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "Assigned name value is greater than 11 characters.");

                _name = value;
            }
        }

        /// <summary>
        ///     Player colour.
        /// </summary>
        public Colour Colour { get; set; } = Colour.White;

        /// <summary>
        ///     Mouse settings.
        /// </summary>
        public Mouse Mouse { get; set; } = new Mouse();

        /// <summary>
        ///     Audio settings.
        /// </summary>
        public Audio Audio { get; set; } = new Audio();

        /// <summary>
        ///     Video settings.
        /// </summary>
        public Video Video { get; set; } = new Video();

        /// <summary>
        ///     Network settings.
        /// </summary>
        public Network Network { get; set; } = new Network();
    }
}