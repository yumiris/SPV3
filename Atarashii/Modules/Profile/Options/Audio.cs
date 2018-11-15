namespace Atarashii.Modules.Profile.Options
{
    public class Audio
    {
        public Volume Volume { get; set; } = new Volume();
        public Quality Quality { get; set; } = new Quality();
        public Quality Variety { get; set; } = new Quality();
    }
}