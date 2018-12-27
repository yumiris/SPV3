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

        private bool _canCreate;

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

        public bool CanCreate
        {
            get => _canCreate;
            set
            {
                if (value == _canCreate) return;
                _canCreate = value;
                OnPropertyChanged();
            }
        }

        public void NotifyCanCreate()
        {
            CanCreate = Directory.Exists(Source) && Directory.Exists(Target);
        }

        public void Create()
        {
            new Compression().Pack(Source, Target);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            NotifyCanCreate();
        }
    }
}