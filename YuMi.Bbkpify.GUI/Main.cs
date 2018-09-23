using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace YuMi.Bbkpify.GUI
{
    public class Main : INotifyPropertyChanged
    {
        /// <summary>
        /// Location of the configuration file used for persistent values.
        /// </summary>
        private static string ConfigFile =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YuMi.Bbkpify.cfg");
        
        private const string BbkpifyExecutable = "YuMi.Bbkpify.CLI.exe";
        private const string UnbbkpifyExecutable = "YuMi.Unbbkpify.CLI.exe";
        private const string SapienExecutable = "os_sapien2.exe";
        
        private string placeholder;
        private string directory;

        private bool nrmlPattern;
        private bool multiPattern;
        private bool diffPattern;

        private bool readyToCommit;
        private bool readyToRevert;

        /// <summary>
        ///     Bitmaps placeholder path.
        /// </summary>
        public string Placeholder
        {
            get => placeholder;
            set
            {
                if (value == placeholder) return;
                placeholder = value;
                NotifyPropertyChanged();
                ValidateProperties();
                SaveConfig();
            }
        }

        /// <summary>
        ///     Bitmaps directory path.
        /// </summary>
        public string Directory
        {
            get => directory;
            set
            {
                if (value == directory) return;
                directory = value;
                NotifyPropertyChanged();
                ValidateProperties();
                SaveConfig();
            }
        }

        /// <summary>
        ///     NRML bitmaps search pattern.
        /// </summary>
        public bool NrmlPattern
        {
            get => nrmlPattern;
            set
            {
                if (value == nrmlPattern) return;
                nrmlPattern = value;
                NotifyPropertyChanged();
                ValidateProperties();
                SaveConfig();
            }
        }

        /// <summary>
        ///     MULTI bitmaps search pattern.
        /// </summary>
        public bool MultiPattern
        {
            get => multiPattern;
            set
            {
                if (value == multiPattern) return;
                multiPattern = value;
                NotifyPropertyChanged();
                ValidateProperties();
                SaveConfig();
            }
        }

        /// <summary>
        ///     DIFF bitmaps search pattern.
        /// </summary>
        public bool DiffPattern
        {
            get => diffPattern;
            set
            {
                if (value == diffPattern) return;
                diffPattern = value;
                NotifyPropertyChanged();
                ValidateProperties();
                SaveConfig();
            }
        }

        /// <summary>
        ///     The requirements to apply the placeholder have been met.
        /// </summary>
        public bool ReadyToCommit
        {
            get => readyToCommit;
            set
            {
                if (value == readyToCommit) return;
                readyToCommit = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     The requirements to revert the placeholder have been met.
        /// </summary>
        public bool ReadyToRevert
        {
            get => readyToRevert;
            set
            {
                if (value == readyToRevert) return;
                readyToRevert = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     Checks if the Sapien executable exists.
        /// </summary>
        public bool CanLoadSapien => File.Exists(SapienExecutable);

        /// <summary>
        ///     Checks if the property values are valid, thereby enabling commands.
        /// </summary>
        private void ValidateProperties()
        {
            ReadyToCommit = PlaceholderValidator.GetStatus(Placeholder) == PlaceholderStatus.IsValid &&
                            DirectoryValidator.GetStatus(Directory) == DirectoryStatus.IsValid &&
                            (NrmlPattern || MultiPattern || DiffPattern);

            ReadyToRevert = DirectoryValidator.GetStatus(Directory) == DirectoryStatus.IsValid;
        }

        public void Commit()
        {
            var args = $"{Placeholder} {Directory}";

            if (NrmlPattern)
            {
                Process.Start(BbkpifyExecutable, $"{args} {Bbkpify.Main.Patterns[0]}");
            }

            if (MultiPattern)
            {
                Process.Start(BbkpifyExecutable, $"{args} {Bbkpify.Main.Patterns[1]}");
            }

            if (DiffPattern)
            {
                Process.Start(BbkpifyExecutable, $"{args} {Bbkpify.Main.Patterns[2]}");
            }
        }

        public void Revert()
        {
            Process.Start(UnbbkpifyExecutable, $"{Directory}");
        }

        public void SaveConfig()
        {
            File.WriteAllText(ConfigFile, $@"{Placeholder}|{Directory}|{NrmlPattern}|{MultiPattern}|{DiffPattern}");
        }

        public void LoadConfig()
        {
            if (!File.Exists(ConfigFile)) return;
            
            var config = File.ReadAllText(ConfigFile).Split('|');
            Placeholder = config[0];
            Directory = config[1];
            NrmlPattern = config[2].Equals("True");
            MultiPattern = config[3].Equals("True");
            DiffPattern = config[4].Equals("True");
        }

        public void LoadSapien()
        {
            if (CanLoadSapien)
            {
                Process.Start(SapienExecutable);
            }
        }

        public void LoadBbkpify()
        {
            Process.Start(BbkpifyExecutable);
        }

        public void LoadUnbbkpify()
        {
            Process.Start(UnbbkpifyExecutable);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}