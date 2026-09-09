using Deployment.Core.Configuration;
using Deployment.Core.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.UI.ViewModels;

public sealed class ServerSelectionViewModel(
    DeploymentConfiguration configuration,
    IServerProvider provider)
{
    public IReadOnlyCollection<ServerInfo> Servers { get; private set; } = [];

    public async Task LoadAsync()
    {
        Servers = await provider.GetServers();
    }

    public void SelectServer(ServerInfo server)
    {
        configuration.Server = server;
    }

    #region Events for MainWindow
    public event Action? ContinueRequest;
    public void Continue()
    {
        if (configuration.Server is null)
            return;

        ContinueRequest?.Invoke();
    }
    #endregion
}
