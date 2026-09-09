using Deployment.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Deployment;

public interface IDeploymentService
{
    /// <summary>
    /// Handles the specified version using the active deployment configuration.
    /// </summary>
    /// <param name="configuration">
    /// The deployment configuration containing the settings required for the deployment.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous deployment operation, containing the results of the deployment.
    /// </returns>
    Task<DeploymentResult> DeployAsync(
        DeploymentConfiguration configuration);
}
