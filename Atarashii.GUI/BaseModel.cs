using System.ComponentModel;
using System.Runtime.CompilerServices;
using Atarashii.GUI.Annotations;

namespace Atarashii.GUI
{
    public class BaseModel : INotifyPropertyChanged
    {
        public LogWindow LogWindow { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}