using System.ComponentModel;
using System.Runtime.CompilerServices;
using SPV3.Compiler.GUI.Annotations;

namespace SPV3.Compiler.GUI
{
    public class Main : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}