using Deployment.Core.Configuration;
using Deployment.Core.Connection;
using Deployment.Core.Deployment;
using Deployment.Core.Deployment.VersionDeployment;
using Deployment.Core.Server;
using Deployment.Core.Version;
using Deployment.Core.Website;
using Deployment.UI.Configuration;
using Deployment.UI.Logging;
using Deployment.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Deployment.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _services = null!;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ServiceCollection services = new();

            services.AddSingleton<DeploymentConfiguration>();

            services.AddSingleton<IDeploymentSettingsProvider, DeploymentSettingsProvider>();
            services.AddSingleton<IDeploymentSettingsRepository, DeploymentSettingsRepository>();

            ServiceProvider initialServices = services.BuildServiceProvider();
            var settingsProvider =
                initialServices.GetRequiredService<IDeploymentSettingsProvider>();

            DeploymentSettings settings =
                await settingsProvider.GetAsync();

            services.AddSingleton(settings);

            services.AddSingleton<IVersionProvider>(
                serviceProvider => new VersionProvider(settings));

            services.AddSingleton<IServerProvider>(
                serviceProver => new ServerProvider(settings));

            services.AddSingleton<IWebsiteProvider>(
                serviceProvider => new WebsiteProvider(settings));

            services.AddSingleton<IConnectionProvider, ConnectionProvider>();
            services.AddSingleton<IConnectionValidator, ConnectionValidator>();
            services.AddSingleton<IVersionDeployment, Version700Deployment>();
            services.AddSingleton<IVersionDeploymentProvider, VersionDeploymentProvider>();
            services.AddSingleton<IDeploymentService, DeploymentService>();
            services.AddSingleton<IDeploymentProgress, DeploymentProgress>();

            services.AddSingleton<DeploymentModeViewModel>();
            services.AddSingleton<VersionSelectionViewModel>();
            services.AddSingleton<ServerSelectionViewModel>();
            services.AddSingleton<WebsiteSelectionViewModel>();
            services.AddSingleton<ConnectionViewModel>();
            services.AddSingleton<PreDeploymentViewModel>();
            services.AddSingleton<DeploymentViewModel>();
            services.AddSingleton<SettingsViewModel>();

            services.AddSingleton<MainWindow>();

            _services = services.BuildServiceProvider();

            var window = _services.GetRequiredService<MainWindow>();
            window.Show();
        }
    }
}
