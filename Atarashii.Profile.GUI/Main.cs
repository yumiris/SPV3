using System;
using System.IO;
using Atarashii.GUI;

namespace Atarashii.Profile.GUI
{
    public class Main : BaseModel
    {
        private string _lastprofFile;

        private string _profileName;

        /// <summary>
        ///     Lastprof text file path.
        /// </summary>
        public string LastprofFile
        {
            get => _lastprofFile;
            set
            {
                if (value == _lastprofFile) return;
                _lastprofFile = value;
                OnPropertyChanged();

                if (string.IsNullOrWhiteSpace(value))
                    LogWindow.Log("Cleared selection.");
                else
                    LogWindow.Log($"Selected {value}.");
            }
        }

        /// <summary>
        ///     HCE executable path.
        /// </summary>
        public string ProfileName
        {
            get => _profileName;
            set
            {
                if (value == _profileName) return;
                _profileName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Invokes the lastprof.txt string data parsing method.
        /// </summary>
        public void ParseLastprofFile()
        {
            if (!File.Exists(LastprofFile))
            {
                LogWindow.Log("No existent lastprof.txt provided.");
                return;
            }

            try
            {
                ProfileName = new Lastprof(File.ReadAllText(LastprofFile)).Parse();
                LogWindow.Log($"Successfully parsed profile name: {ProfileName}");
            }
            catch (ProfileException e)
            {
                LogWindow.Log(e.Message);
            }
        }

        /// <summary>
        ///     Invokes the lastprof.txt detection method and parses its contents.
        /// </summary>
        public void DetectLastprof()
        {
            try
            {
                ProfileName = LastprofFactory.Get(LastprofFactory.Type.Detect).Parse();
                LogWindow.Log($"Successfully detected lastprof.txt and parsed profile name: {ProfileName}");
            }
            catch (Exception e)
            {
                if (e is ProfileException || e is FileNotFoundException)
                    LogWindow.Log(e.Message);
            }
        }
    }
}