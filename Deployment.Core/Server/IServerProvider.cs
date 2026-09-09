using Deployment.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Server;

public interface IServerProvider
{
    /// <summary>
    /// Gets the list of servers inputted in the deployment application settings.
    /// </summary>
    /// <returns>
    /// A read-only list of server information.
    /// </returns>
    Task<IReadOnlyList<ServerInfo>> GetServers();
}
