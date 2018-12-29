using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using SPV3.Compiler.Properties;

namespace SPV3.Compiler
{
    /// <summary>
    ///     Main model used for the UI bindings.
    /// </summary>
    public class Main : INotifyPropertyChanged
    {
        private string _source;
        private string _target;

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