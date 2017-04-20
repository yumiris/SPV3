using System.Diagnostics;
using System.IO;

namespace Promise.Library.Halo
{
    public class Launch
    {
        private const string ExecutableName = "haloce.exe";
        private readonly Halo _halo;

        public Launch(Halo halo)
        {
            _halo = halo;
        }

        public void Start()
        {
            if (File.Exists(ExecutableName))
                Process.Start(ExecutableName, GetLaunchParameters());
            else
                throw new FileNotFoundException("Halo executable has not been found in this directory.");
        }

        private string GetLaunchParameters()
        {
            string windowMode = _halo.IsWindow ? "-window" : string.Empty;
            string safeMode = _halo.IsSafeMode ? "-safemode" : string.Empty;
            string fixedMode = _halo.IsFixedMode ? "-useff" : string.Empty;

            string resolution =
                $"{_halo.VideoResolution.Width},{_halo.VideoResolution.Height},{_halo.VideoRefreshRate.Rate}";
            string toggles = $"{windowMode} {safeMode} {fixedMode}";

            return $"-vidmode {resolution} -adapter {_halo.VideoAdapter.Index} {toggles}";
        }
    }
}