using System.IO;

namespace YuMi.Bbkpify
{
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
}
