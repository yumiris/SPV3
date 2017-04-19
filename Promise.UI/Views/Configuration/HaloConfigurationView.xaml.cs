using System.Windows;
using System.Windows.Controls;
using Promise.Library.Halo.Video;
using Promise.UI.Controller;

namespace Promise.UI.Views.Configuration
{
    /// <summary>
    ///     Interaction logic for ConfigView.xaml
    /// </summary>
    public partial class HaloConfigurationView : Window
    {
        private readonly HaloConfigurationController _haloConfigurationController = new HaloConfigurationController();

        public HaloConfigurationView()
        {
            InitializeComponent();
            DataContext = _haloConfigurationController;
            _haloConfigurationController.GetConfiguration();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _haloConfigurationController.SaveConfiguration();
            }
            catch
            {
                MessageBox.Show("Hmm, looks like we can't save. Try running as an Administrator, please!");
            }

            Close();
        }

        private void ResolutionsComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ResolutionsComboBox.ItemsSource = new VideoResolution().GetVideoResolutions();
        }

        private void RefreshRatesComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshRatesComboBox.ItemsSource = new VideoRefreshRate().GetVideoRefreshRates();
        }

        private void AdaptersComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            AdaptersComboBox.ItemsSource = new VideoAdapter().GetAdaptersList();
        }

        private void AdaptersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _haloConfigurationController.SelectedAdapterIndex = AdaptersComboBox.SelectedIndex;
        }

        private void ResolutionsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _haloConfigurationController.SelectedVideoResolution = ResolutionsComboBox.SelectedItem as VideoResolution;
        }

        private void RefreshRatesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _haloConfigurationController.SelectedVideoRefreshRate =
                RefreshRatesComboBox.SelectedItem as VideoRefreshRate;
        }

        private void OSConfigButton_Click(object sender, RoutedEventArgs e)
        {
            new OsConfigurationView().Show();
        }
    }
}