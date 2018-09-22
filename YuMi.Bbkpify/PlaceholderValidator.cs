using System.IO;

namespace YuMi.Bbkpify
{
    public static class PlaceholderValidator
    {
        /// <summary>
        ///     Maximum placeholder file size.
        /// </summary>
        private const int SafeFileSize = 0x800000;

        /// <summary>
        ///     Returns the validity status of a given placeholder.
        /// </summary>
        /// <param name="placeholderPath">
        ///     Path of the placeholder file.
        /// </param>
        /// <returns>
        ///     PlaceholderStatus enum value.
        /// </returns>
        public static PlaceholderStatus GetStatus(string placeholderPath)
        {
            if (!File.Exists(placeholderPath))
            {
                return PlaceholderStatus.DoesNotExist;
            }

            if (new FileInfo(placeholderPath).Length > SafeFileSize)
            {
                return PlaceholderStatus.IsTooLarge;
            }

            return PlaceholderStatus.IsValid;
        }
    }
}
