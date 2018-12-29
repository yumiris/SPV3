using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SPV3.Compiler.GUI.Annotations;
using SPV3.Domain;
using Directory = System.IO.Directory;

namespace SPV3.Compiler.GUI
{
    /// <summary>
    ///     Main model used for the UI bindings.
    /// </summary>
    public class Main : INotifyPropertyChanged, IStatus
    {
        private string _source;
        private string _target;

        private string _status;

        public string Status
        {
            get => _status;
            set
            {
                if (value == _status) return;
                _status = value;
                OnPropertyChanged();
            }
        }

        private bool _canCompile;

        /// <summary>
        ///     Source directory path.
        /// </summary>
        public string Source
        {
            get => _source;
            set
            {
                if (value == _source) return;
                _source = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Target directory path.
        /// </summary>
        public string Target
        {
            get => _target;
            set
            {
                if (value == _target) return;
                _target = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Allow compilation.
        /// </summary>
        public bool CanCompile
        {
            get => _canCompile;
            set
            {
                if (value == _canCompile) return;
                _canCompile = value;
                OnPropertyChanged();
            }
        }
        
        public void CommitStatus(string data)
        {
            Status += $"\n{DateTime.Now:u} - {data}";
        }

        /// <summary>
        ///     Updates CanCompile. If Source & Target directories exist on the filesystem, CanCompile becomes true.
        /// </summary>
        public void NotifyCanCompile()
        {
            CanCompile = Directory.Exists(Source) && Directory.Exists(Target);
        }

        /// <summary>
        ///     Instantiates the SPV3.Compiler's Compiler class with the given Source & Target directory, and invokes
        ///     the Compile method.
        /// </summary>
        public void Compile()
        {
            new Compiler((SPV3.Domain.Directory) Source, (SPV3.Domain.Directory) Target).Compile();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            NotifyCanCompile();
        }
    }
}