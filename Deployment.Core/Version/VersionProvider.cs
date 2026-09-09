using Deployment.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Version;

public sealed class VersionProvider(DeploymentSettings settings) : IVersionProvider
{
    public Task<IReadOnlyList<VersionInfo>> GetVersionsAsync()
    {
        if (string.IsNullOrWhiteSpace(settings.UpdateSource))
            throw new ArgumentException("The 'UpdateSource' property has not been configured correctly.", nameof(settings));

        if (!Directory.Exists(settings.UpdateSource))
            throw new DirectoryNotFoundException("The path in 'UpdateSource' does not exist.");

        string[] directories = Directory.GetDirectories(settings.UpdateSource, "*.*.*", SearchOption.TopDirectoryOnly);
        List<VersionInfo> versions = [];

        foreach (var directory in directories)
        {
            string version = Path.GetFileName(directory);
            versions.Add(new(version, directory));
        }

        return Task.FromResult<IReadOnlyList<VersionInfo>>([.. versions.OrderByDescending(v => v.Value)]);
    }
}
