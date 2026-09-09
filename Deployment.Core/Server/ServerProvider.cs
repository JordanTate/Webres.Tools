using Deployment.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Server;

public sealed class ServerProvider(DeploymentSettings settings) : IServerProvider
{
    public Task<IReadOnlyList<ServerInfo>> GetServers()
    {
        if (settings.WebServers.Length == 0)
            throw new ArgumentException("No server names have been added to the 'WebServers' property.", nameof(settings));

        List<ServerInfo> servers = [];
        foreach (string serverName in settings.WebServers)
        {
            string path = $"{serverName}\\";
            servers.Add(new(serverName, path));
        }

        return Task.FromResult<IReadOnlyList<ServerInfo>>([.. servers.OrderBy(s => s.Name)]);
    }
}
