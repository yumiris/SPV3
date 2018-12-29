namespace SPV3.Domain
{
    /// <summary>
    ///     Interface dealing with a progress status. This should be implemented by a type that binds or outputs data
    ///     to the user. For example, a ViewModel can implement this interface by assigning the data to a property that
    ///     is bound to a view control.
    /// </summary>
    public interface IStatus
    {
        /// <summary>
        ///     Appends given progress to the current status.
        /// </summary>
        /// <param name="data">
        ///     Data to append the status.
        /// </param>
        void Commit(string data);
    }
}