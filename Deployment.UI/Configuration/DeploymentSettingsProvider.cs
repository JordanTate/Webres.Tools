using Deployment.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.UI.Configuration;

/// <summary>
/// Provides access to deployment application settings and handles Read and Write actions.
/// </summary>
/// <param name="repository">
/// The repository used to retrieve and persist deployment application settings.
/// </param>
public class DeploymentSettingsProvider(IDeploymentSettingsRepository repository) : IDeploymentSettingsProvider
{
    public async Task<DeploymentSettings> GetAsync()
    {
        var result = await repository.GetAsync();

        return result is null ? new() : result;
    }

    public async Task SaveAsync(DeploymentSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.UpdateSource)
            || settings.WebServers.Length < 1
            || string.IsNullOrWhiteSpace(settings.WebsiteRoot))
            return;

        await repository.SaveAsync(settings);
    }
}
