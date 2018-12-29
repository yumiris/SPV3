namespace SPV3.Domain
{
    /// <summary>
    ///     Abstract representing a progress status.
    /// </summary>
    public abstract class Status
    {
        /// <summary>
        ///     Appends given progress to the current status.
        /// </summary>
        /// <param name="data">
        ///     Data to append the status.
        /// </param>
        public abstract void Append(string data);

        /// <summary>
        ///     Resets the status to the given progress.
        /// </summary>
        /// <param name="data">
        ///     Data to update the status to.
        /// </param>
        public abstract void Update(string data);
    }
}