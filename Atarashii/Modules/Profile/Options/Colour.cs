namespace Atarashii.Modules.Profile.Options
{
    public class Colour
    {
        public enum Type
        {
            White,
            Black,
            Red,
            Blue,
            Gray,
            Yellow,
            Green,
            Pink,
            Purple,
            Cyan,
            Cobalt,
            Oragne,
            Teal,
            Sage,
            Brown,
            Tan,
            Maroon,
            Salmon
        }

        public Type Value { get; set; } = Type.White;
    }
}