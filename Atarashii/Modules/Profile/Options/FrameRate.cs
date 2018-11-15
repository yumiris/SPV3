namespace Atarashii.Modules.Profile.Options
{
    /// <summary>
    ///     Representation of the video frame rate settings.
    /// </summary>
    public class FrameRate
    {
        /// <summary>
        ///     Available frame rate types.
        /// </summary>
        public enum Type
        {
            Fps30,
            VsyncOn,
            VsyncOff
        }

        /// <summary>
        ///     Frame rate type value.
        /// </summary>
        public Type Value { get; set; } = Type.Fps30;
    }
}