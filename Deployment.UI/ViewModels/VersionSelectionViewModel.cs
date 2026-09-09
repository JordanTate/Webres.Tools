using Deployment.Core.Configuration;
using Deployment.Core.Version;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.UI.ViewModels;

public sealed class VersionSelectionViewModel(
    DeploymentConfiguration configuration,
    IVersionProvider versionProvider)
{
    public IReadOnlyCollection<VersionInfo> Versions { get; private set; } = [];

    public async Task LoadAsync()
    {
        Versions = await versionProvider.GetVersionsAsync();
    }

    public void SelectVersion(VersionInfo version)
    {
        configuration.Version = version;
    }

    #region Events for MainWindow
    public event Action? ContinueRequested;
    public void Continue()
    {
        if (configuration.Version is null)
            return;

        ContinueRequested?.Invoke();
    }
    #endregion
}
