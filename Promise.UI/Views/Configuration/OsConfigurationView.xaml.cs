using System.Windows;
using Promise.UI.Controller;

namespace Promise.UI.Views.Configuration
{
    /// <summary>
    ///     Interaction logic for OSConfigurationView.xaml
    /// </summary>
    public partial class OsConfigurationView : Window
    {
        private readonly OsConfigurationController _osConfigurationController = new OsConfigurationController();

        public OsConfigurationView()
        {
            InitializeComponent();
            DataContext = _osConfigurationController;
            _osConfigurationController.GetConfiguration();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _osConfigurationController.SaveConfiguration();
            Close();
        }
    }
}