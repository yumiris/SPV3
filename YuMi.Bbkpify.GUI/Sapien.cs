using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace YuMi.Bbkpify.GUI
{
    public class Sapien : INotifyPropertyChanged
    {
        private string path;

        public string Path
        {
            get => path;
            set
            {
                if (value == path) return;
                path = value;
                NotifyPropertyChanged();
            }
        }
        
        public void SavePath()
        {
            Configuration.Save(new Configuration
            {
                SapienExecutable = Path
            });
        }

        public void LoadPath()
        {
            Path = Configuration.Load().SapienExecutable;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}