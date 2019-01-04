namespace SPV3.Domain
{
    /// <summary>
    ///     Binary file size.
    /// </summary>
    public class Size
    {
        /// <summary>
        ///     Value of the binary file size.
        /// </summary>
        public long Value { get; set; }

        /// <summary>
        ///     Represent object as long.
        /// </summary>
        /// <param name="size">
        ///     Object to represent as long.
        /// </param>
        /// <returns>
        ///     Long representation of the object.
        /// </returns>
        public static implicit operator long(Size size)
        {
            return size.Value;
        }

        /// <summary>
        ///     Represent long as object.
        /// </summary>
        /// <param name="value">
        ///     Long to represent as object.
        /// </param>
        /// <returns>
        ///     Object representation of the long.
        /// </returns>
        public static explicit operator Size(long value)
        {
            return new Size
            {
                Value = value
            };
        }
    }
}