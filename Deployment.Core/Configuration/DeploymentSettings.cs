using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Configuration;

public sealed class DeploymentSettings
{
    public string UpdateSource { get; set; } = string.Empty;
    public string[] WebServers { get; set; } = [];
    public string WebsiteRoot { get; set; } = string.Empty;
}
