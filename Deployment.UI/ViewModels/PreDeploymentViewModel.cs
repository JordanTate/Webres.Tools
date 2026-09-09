using Deployment.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.UI.ViewModels;

public sealed class PreDeploymentViewModel(
    DeploymentConfiguration configuration)
{
    public DeploymentConfiguration Configuration { get; set; } = configuration;

    #region Events for MainWindow
    public event Action? DeployRequested;

    public void Deploy()
    {
        DeployRequested?.Invoke();
    }
    #endregion
}
