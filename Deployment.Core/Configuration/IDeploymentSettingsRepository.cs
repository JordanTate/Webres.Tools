using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Configuration;

public interface IDeploymentSettingsRepository
{
    /// <summary>
    /// Retrieves the stored deployment settings.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous retrieval operation, containing the stored
    /// deployment settings, or <see langword="null"/> if no settings have been stored.
    /// </returns>
    Task<DeploymentSettings?> GetAsync();

    /// <summary>
    /// Persists the specified deployment settings.
    /// </summary>
    /// <param name="settings">
    /// The deployment settings to save.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous save operation.
    /// </returns>
    Task SaveAsync(DeploymentSettings settings);
}
