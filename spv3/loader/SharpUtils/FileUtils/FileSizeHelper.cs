using System;

namespace SharpUtils.FileUtils
{
    public static class FileSizeHelper
    {
        /// <summary>
        /// Converts file size in bytes to human readable string.
        /// </summary>
        /// <param name="Bytes">The filesize in bytes.</param>
        /// <returns>The string representation.</returns>
        public static string GetHumanReadableSize(long Bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = Bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            
            return String.Format("{0:0.#}{1}", len, sizes[order]);
        }
    }
}
