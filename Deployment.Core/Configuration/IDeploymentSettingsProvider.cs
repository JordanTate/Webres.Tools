using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Configuration;

public interface IDeploymentSettingsProvider
{
    /// <summary>
    /// Retrieves the current deployment settings.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous retrieval operation, containing the
    /// current deployment settings.
    /// </returns>
    Task<DeploymentSettings> GetAsync();

    /// <summary>
    /// Saves the specified deployment settings.
    /// </summary>
    /// <param name="settings">
    /// The deployment settings to save.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous save operation.
    /// </returns>
    Task SaveAsync(DeploymentSettings settings);
}
