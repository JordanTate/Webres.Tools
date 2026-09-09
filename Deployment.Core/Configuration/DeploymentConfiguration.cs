using Deployment.Core.Connection;
using Deployment.Core.Server;
using Deployment.Core.Version;
using Deployment.Core.Website;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Configuration;

public sealed class DeploymentConfiguration
{
    public DeploymentMode Mode { get; set; }
    public VersionInfo? Version { get; set; }
    public ServerInfo? Server { get; set; }
    public WebsiteInfo? Website { get; set; }
    public ConnectionInfo? Connection { get; set; }
}
