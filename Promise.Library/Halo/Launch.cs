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
            if (!File.Exists(ExecutableName))
                throw new FileNotFoundException("Halo executable has not been found in this directory.");

            Process.Start(ExecutableName, GetLaunchParameters());
        }

        private string GetLaunchParameters()
        {
            string windowMode = GetParameterValue("-window", _halo.IsWindow);
            string safeMode = GetParameterValue("-safemode", _halo.IsSafeMode);
            string fixedMode = GetParameterValue("useff", _halo.IsFixedMode);
            string toggles = $"{windowMode} {safeMode} {fixedMode}";

            string resolution =
                $"{_halo.VideoResolution.Width},{_halo.VideoResolution.Height},{_halo.VideoRefreshRate.Rate}";

            return $"-vidmode {resolution} -adapter {_halo.VideoAdapter.Index} {toggles}";
        }

        private string GetParameterValue(string parameter, bool toggle)
        {
            return (toggle) ? parameter : string.Empty;
        }
    }
}