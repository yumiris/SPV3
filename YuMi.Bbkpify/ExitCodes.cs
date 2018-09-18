namespace YuMi.Bbkpify
{
    /// <summary>
    ///     Main application exit codes.
    /// </summary>
    public enum ExitCodes
    {
        Success = 0,
        InvalidPlaceholderPath,
        InvalidFilesFolderPath,
        InvalidFileNamePattern,
        ExceptionHasBeenThrown,
        PlaceholderFileTooLong
    }
}