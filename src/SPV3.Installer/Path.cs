namespace SPV3.Installer
{
    /// <summary>
    ///     Represents the absolute path of a directory or file.
    /// </summary>
    public class Path
    {
        /// <example>
        ///     C:\SPV3.2\maps
        /// </example>
        /// <example>
        ///     C:\SPV3.2\maps\loc.map
        /// </example>
        public string Value { get; set; }
    }
}