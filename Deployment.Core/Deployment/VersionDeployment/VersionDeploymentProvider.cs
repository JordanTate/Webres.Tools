using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Deployment.VersionDeployment;

public sealed class VersionDeploymentProvider(
    IEnumerable<IVersionDeployment> deployments) : IVersionDeploymentProvider
{
    public IVersionDeployment? Get(string version)
    {
        return deployments.FirstOrDefault(
            deployment => string.Equals(
                deployment.Version,
                version,
                StringComparison.OrdinalIgnoreCase));
    }
}
