namespace SPV3.Domain
{
    /// <summary>
    ///     Abstract representing a progress status.
    ///     This status can be injected into library routines to assign their progress in a human-readable format.
    ///     The assigned data can be displayed to, for example, the CLI, or be bound to a View.
    /// </summary>
    public abstract class Status
    {
        /// <summary>
        ///     Appends given progress to the current status.
        /// </summary>
        /// <param name="data">
        ///     Data to append the status.
        /// </param>
        public abstract void Commit(string data);
    }
}