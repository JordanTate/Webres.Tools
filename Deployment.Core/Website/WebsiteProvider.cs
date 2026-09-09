using Deployment.Core.Configuration;
using Deployment.Core.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Website;

public sealed class WebsiteProvider(DeploymentSettings settings) : IWebsiteProvider
{
    private const string FILE_WEB_CONFIG = "Web.Config";

    public Task<IReadOnlyList<WebsiteInfo>> GetWebsitesByServer(
        ServerInfo server)
    {
        if (string.IsNullOrWhiteSpace(settings.WebsiteRoot))
            throw new ArgumentException("The 'WebsiteRoot' property has not been configured correctly", nameof(settings));

        string rootPath = $"{server.Path}{settings.WebsiteRoot}";

        if (!Directory.Exists(rootPath))
            throw new DirectoryNotFoundException($"The website root directory '{rootPath}' does not exist.");

        List<WebsiteInfo> websites = [];

        foreach (string directory in Directory.GetDirectories(rootPath))
        {
            string webConfig = Path.Combine(
                directory, FILE_WEB_CONFIG);

            string name = Path.GetFileName(directory);

            if (File.Exists(webConfig))
                websites.Add(new(name, directory));
        }

        return Task.FromResult<IReadOnlyList<WebsiteInfo>>([.. websites.OrderBy(w => w.Name)]);
    }
}
