using Promise.Library;
using PropertyChanged;

namespace Promise.UI.Controller
{
    [ImplementPropertyChanged]
    class ConfigurationController : Configuration
    {
        public int ChosenAdapter { get { return Adapter - 1; } set { Adapter = value + 1; } }
    }
}
