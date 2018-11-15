namespace Atarashii.Modules.Profile.Options
{
    public class FrameRate
    {
        public enum Type
        {
            Fps30,
            VsyncOn,
            VsyncOff
        }

        public Type Value { get; set; } = Type.Fps30;
    }
}