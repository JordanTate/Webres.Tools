
using Deployment.Core.Configuration;
using Deployment.UI.ViewModels;
using Deployment.UI.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Appearance;

namespace Deployment.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.FluentWindow
    {
        private readonly DeploymentConfiguration _configuration;
        private readonly DeploymentSettings _settings;
        private readonly DeploymentModeViewModel _deploymentModeViewModel;
        private readonly VersionSelectionViewModel _versionSelectionViewModel;
        private readonly ServerSelectionViewModel _serverSelectionViewModel;
        private readonly WebsiteSelectionViewModel _websiteSelectionViewModel;
        private readonly ConnectionViewModel _connectionViewModel;
        private readonly PreDeploymentViewModel _preDeploymentViewModel;
        private readonly DeploymentViewModel _deploymentViewModel;
        private readonly SettingsViewModel _settingsViewModel;
        public MainWindow(
            DeploymentConfiguration configuration,
            DeploymentSettings settings,
            DeploymentModeViewModel deploymentModeViewModel,
            VersionSelectionViewModel versionSelectionViewModel,
            ServerSelectionViewModel serverSelectionViewModel,
            WebsiteSelectionViewModel websiteSelectionViewModel,
            ConnectionViewModel connectionViewModel,
            PreDeploymentViewModel preDeploymentViewModel,
            DeploymentViewModel deploymentViewModel,
            SettingsViewModel settingsViewModel)
        {
            InitializeComponent();

            _configuration = configuration;
            _settings = settings;
            _settingsViewModel = settingsViewModel;
            _deploymentModeViewModel = deploymentModeViewModel;
            _versionSelectionViewModel = versionSelectionViewModel;
            _serverSelectionViewModel = serverSelectionViewModel;
            _websiteSelectionViewModel = websiteSelectionViewModel;
            _connectionViewModel = connectionViewModel;
            _preDeploymentViewModel = preDeploymentViewModel;
            _deploymentViewModel = deploymentViewModel;

            _settingsViewModel.SaveRequested += OnSettingsSaved;
            _deploymentModeViewModel.ModeSelected += OnModeSelected;
            _versionSelectionViewModel.ContinueRequested += OnVersionSelected;
            _serverSelectionViewModel.ContinueRequest += OnServerSelected;
            _websiteSelectionViewModel.ContinueRequested += OnWebsiteSelection;
            _connectionViewModel.ContinueRequested += OnConnectionValidated;
            _preDeploymentViewModel.DeployRequested += OnDeploy;
            _deploymentViewModel.NewDeploymentRequested += OnNewDeployment;
            _deploymentViewModel.CloseRequested += OnCloseRequested;

            if (IsConfigured())
                OnStart();
            else
                MainContent.Content = new SettingsView(
                    _settingsViewModel);
        }

        private void OnStart()
        {
            MainContent.Content = new DeploymentModeView(
                    _deploymentModeViewModel);
        }

        #region Callbacks
        private async void OnModeSelected()
        {
            await _versionSelectionViewModel.LoadAsync();
            MainContent.Content = new VersionSelectionView(_versionSelectionViewModel);
        }

        private async void OnVersionSelected()
        {
            await _serverSelectionViewModel.LoadAsync();
            MainContent.Content = new ServerSelectionView(_serverSelectionViewModel);
        }

        private async void OnServerSelected()
        {
            await _websiteSelectionViewModel.LoadAsync();
            MainContent.Content = new WebsiteSelectionView(_websiteSelectionViewModel);
        }

        private async void OnWebsiteSelection()
        {
            await _connectionViewModel.LoadAsync();
            MainContent.Content = new ConnectionView(_connectionViewModel);
        }

        private async void OnConnectionValidated()
        {
            MainContent.Content = new PreDeploymentView(_preDeploymentViewModel);
        }

        private async void OnDeploy()
        {
            MainContent.Content = new DeploymentView(_deploymentViewModel);
        }

        private void OnNewDeployment()
        {
            _configuration.Version = null;
            _configuration.Server = null;
            _configuration.Website = null;
            _configuration.Connection = null;

            _connectionViewModel.Reset();
            _deploymentViewModel.Reset();

            OnStart();
        }

        private void OnCloseRequested()
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region Settings Configuration Logic
        private bool IsConfigured()
        {
            return !string.IsNullOrWhiteSpace(_settings.UpdateSource)
                && _settings.WebServers.Length > 0
                && !string.IsNullOrWhiteSpace(_settings.WebsiteRoot);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SettingsView(
                    _settingsViewModel);
        }
        private void OnSettingsSaved()
        {
            if (IsConfigured())
            {
                OnStart();
            }
        }
        #endregion
    }
}