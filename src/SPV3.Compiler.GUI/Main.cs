using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using SPV3.Compiler.GUI.Annotations;

namespace SPV3.Compiler.GUI
{
    public class Main : INotifyPropertyChanged
    {
        private string _source;
        private string _target;

        private bool _canCompile;

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

        public void NotifyCanCompile()
        {
            CanCompile = Directory.Exists(Source) && Directory.Exists(Target);
        }

        public void Compile()
        {
            new Compression().Pack(Source, Target);
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