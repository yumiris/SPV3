using System.Windows;
using Promise.UI.Controller;

namespace Promise.UI.Views
{
    /// <summary>
    /// Interaction logic for ConfigView.xaml
    /// </summary>
    public partial class ConfigView : Window
    {
        readonly ConfigurationController _configurationController = new ConfigurationController();

        public ConfigView()
        {
            InitializeComponent();
            DataContext = _configurationController;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _configurationController.WriteConfiguration();
            Close();
        }
    }
}
