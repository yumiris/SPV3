using System.IO;

namespace YuMi.Bbkpify
{
    /// <summary>
    ///     Bitmap placeholder file validation checking.
    /// </summary>
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

    /// <summary>
    ///     Bitmaps directory validation checking.
    /// </summary>
    public static class DirectoryValidator
    {
        /// <summary>
        ///     Returns the validity status of a given directory.
        /// </summary>
        /// <param name="directoryPath">
        ///     Path to the bitmaps directory.
        /// </param>
        /// <returns>
        ///     DirectoryStatus enum value.
        /// </returns>
        public static DirectoryStatus GetStatus(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return DirectoryStatus.DoesNotExist;
            }

            return DirectoryStatus.IsValid;
        }
    }

    /// <summary>
    ///     Search pattern validation checking.
    /// </summary>
    public static class PatternValidator
    {
        /// <summary>
        ///     Returns the validity status of a given bitmap search pattern.
        /// </summary>
        /// <param name="searchPattern">
        ///     Bitmap file search pattern.
        /// </param>
        /// <returns>
        ///     PatternStatus enum value.
        /// </returns>
        public static PatternStatus GetStatus(string searchPattern)
        {
            if (!Main.Patterns.Contains(searchPattern))
            {
                return PatternStatus.IsInvalid;
            }

            return PatternStatus.IsValid;
        }
    }
}
