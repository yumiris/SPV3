namespace Atarashii.Modules.Profile.Options
{
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