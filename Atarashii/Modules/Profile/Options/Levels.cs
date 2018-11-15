namespace Atarashii.Modules.Profile.Options
{
    public class Levels
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