namespace SPV3.Loader
{
    /// <summary>
    ///     Type representing the routines which the Loader should conduct or skip.
    /// </summary>
    public class LoaderConfiguration
    {
        /// <summary>
        ///     Skips verification of the provided HCE executable
        /// </summary>
        public bool SkipVerification { get; set; }
    }
}