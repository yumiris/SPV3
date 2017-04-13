using System.Windows;
using Promise.Library;
using Promise.UI.Controller;

namespace Promise.UI.Views
{
    /// <summary>
    ///     Interaction logic for ConfigView.xaml
    /// </summary>
    public partial class ConfigurationView : Window
    {
        private readonly ConfigurationController _configurationController = new ConfigurationController();

        public ConfigurationView()
        {
            InitializeComponent();
            DataContext = _configurationController;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _configurationController.SaveConfiguration();
            Close();
        }
    }
}