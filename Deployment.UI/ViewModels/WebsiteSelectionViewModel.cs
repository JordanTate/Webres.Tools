using Deployment.Core.Configuration;
using Deployment.Core.Website;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.UI.ViewModels;

public sealed class WebsiteSelectionViewModel(
    DeploymentConfiguration configuration,
    IWebsiteProvider provider)
{
    public IReadOnlyCollection<WebsiteInfo> Websites { get; private set; } = [];

    public async Task LoadAsync()
    {
        Websites = await provider.GetWebsitesByServer(configuration.Server!);
    }

    public void SelectWebsite(WebsiteInfo website)
    {
        configuration.Website = website;
    }

    #region Events for MainWindow
    public event Action? ContinueRequested;
    public void Continue()
    {
        if (configuration.Website is null)
            return;

        ContinueRequested?.Invoke();
    }
    #endregion
}
