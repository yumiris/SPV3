namespace YuMi.Bbkpify
{
    /// <summary>
    ///     Common validity statuses for a given bitmaps directory.
    /// </summary>
    public enum DirectoryStatus
    {
        /// <summary>
        ///     Bitmaps directory is valid and exists.
        /// </summary>
        IsValid,

        /// <summary>
        ///     Bitmaps directory does not exist.
        /// </summary>
        DoesNotExist
    }

    /// <summary>
    ///     Common validity statuses for a given placeholder file.
    /// </summary>
    public enum PlaceholderStatus
    {
        /// <summary>
        ///     Placeholder file is valid in existence and size.
        /// </summary>
        IsValid,

        /// <summary>
        ///     Placeholder file does not exist at the given path.
        /// </summary>
        DoesNotExist,

        /// <summary>
        ///     Placeholder file is larger than 8MiB.
        /// </summary>
        IsTooLarge
    }

    public enum PatternStatus
    {
        /// <summary>
        ///     Bitmaps search pattern is valid.
        /// </summary>
        IsValid,

        /// <summary>
        ///     Bitmaps search pattern is invalid.
        /// </summary>
        IsInvalid
    }
}
