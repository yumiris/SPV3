using System.Windows;
using Promise.UI.Controller;

namespace Promise.UI.Views
{
    /// <summary>
    /// Interaction logic for OSConfigurationView.xaml
    /// </summary>
    public partial class OsConfigurationView : Window
    {
        private readonly OsConfigurationController _osConfigurationController = new OsConfigurationController();

        public OsConfigurationView()
        {
            InitializeComponent();
            DataContext = _osConfigurationController;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _osConfigurationController.SaveData();
//            MessageBox.Show($"{_osConfigurationController.FieldOfView}");
        }
    }
}
