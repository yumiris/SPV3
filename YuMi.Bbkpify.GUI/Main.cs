using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace YuMi.Bbkpify.GUI
{
    public class Main : INotifyPropertyChanged
    {   
        private const string BbkpifyExecutable = "YuMi.Bbkpify.CLI.exe";
        private const string UnbbkpifyExecutable = "YuMi.Unbbkpify.CLI.exe";
        
        private string placeholder;
        private string directory;

        private bool nrmlPattern;
        private bool multiPattern;
        private bool diffPattern;

        private bool readyToCommit;
        private bool readyToRevert;

        private string sapienExecutable;
        
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
        ///     Path of the Sapien executable.
        /// </summary>
        public string SapienExecutable
        {
            get => sapienExecutable;
            set
            {
                if (value == sapienExecutable) return;
                sapienExecutable = value;
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
            Configuration.Save(new Configuration
            {
                Placeholder = Placeholder,
                Directory = Directory,
                NrmlPattern = NrmlPattern,
                MultiPattern = MultiPattern,
                DiffPattern = DiffPattern,
                SapienExecutable = SapienExecutable
            });
        }

        public void LoadConfig()
        {
            var config = Configuration.Load();
            Placeholder = config.Placeholder;
            Directory = config.Directory;
            NrmlPattern = config.NrmlPattern;
            MultiPattern = config.MultiPattern;
            DiffPattern = config.DiffPattern;
            SapienExecutable = config.SapienExecutable;
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