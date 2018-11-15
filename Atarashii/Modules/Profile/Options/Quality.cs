namespace Atarashii.Modules.Profile.Options
{
    /// <summary>
    ///     Representation of a generic quality-type value, e.g. texture, audio.
    /// </summary>
    public class Quality
    {
        public enum Type
        {
            Low,
            Medium,
            High
        }

        public Type Value { get; set; } = Type.High;
    }
}