using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Deployment.VersionDeployment;

public interface IVersionDeploymentProvider
{
    /// <summary>
    /// Retrieves the deployment implementation for the specified version.
    /// </summary>
    /// <param name="version">
    /// The version for which to retrieve a deployment implementation.
    /// </param>
    /// <returns>
    /// The deployment implementation for the specified version, or <see langword="null"/>
    /// if no deployment implementation is available.
    /// </returns>
    IVersionDeployment? Get(string version);
}
