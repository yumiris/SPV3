namespace Promise.Library.OpenSauce.Networking
{
    public class Networking
    {
        public GameSpy GameSpy { get; set; } = new GameSpy();
        public MapDownload MapDownload { get; set; } = new MapDownload();
        public VersionCheck VersionCheck { get; set; } = new VersionCheck();
    }
}