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
            var windowMode = _halo.IsWindow ? "-window" : string.Empty;
            var safeMode = _halo.IsSafeMode ? "-safemode" : string.Empty;
            var fixedMode = _halo.IsFixedMode ? "-useff" : string.Empty;

            var resolution =
                $"{_halo.VideoResolution.Width},{_halo.VideoResolution.Height},{_halo.VideoRefreshRate.Rate}";
            var toggles = $"{windowMode} {safeMode} {fixedMode}";

            return $"-vidmode {resolution} -adapter {_halo.VideoAdapter.Index} {toggles}";
        }
    }
}