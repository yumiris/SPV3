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
        /// <summary>
        ///     <see cref="CanCompile" />
        /// </summary>
        private bool _canCompile;

        /// <summary>
        ///     <see cref="Source" />
        /// </summary>
        private string _source;

        /// <summary>
        ///     <see cref="Status" />
        /// </summary>
        private string _status = "Awaiting end-user invocation...";

        /// <summary>
        ///     <see cref="Target" />
        /// </summary>
        private string _target;

        /// <summary>
        ///     <see cref="Compressor"/>
        /// </summary>
        private Compressor _compressor;

        /// <summary>
        ///     Status output.
        /// </summary>
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

        public Compressor Compressor
        {
            get => _compressor;
            set
            {
                if (Equals(value, _compressor)) return;
                _compressor = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CommitStatus(string data)
        {
            Status = $"{DateTime.Now:u} - {data}\n{Status}";
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
            try
            {
                if (_compressor == null)
                    throw new NullReferenceException("Cannot compile without Compressor instance.");

                var source = (Domain.Directory) Source;
                var target = (Domain.Directory) Target;
                var status = this;

                new Compiler(source, target, _compressor, status).Compile();
            }
            catch (Exception exception)
            {
                CommitStatus(exception.ToString());
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            NotifyCanCompile();
        }
    }
}