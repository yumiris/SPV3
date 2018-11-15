namespace Atarashii.Modules.Profile.Options
{
    public class Particles
    {
        public enum Type
        {
            Off,
            Low,
            High
        }

        public Type Value { get; set; } = Type.High;
    }
}