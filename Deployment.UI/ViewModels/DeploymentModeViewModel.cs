using Deployment.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.UI.ViewModels;

public sealed class DeploymentModeViewModel(
    DeploymentConfiguration configuration)
{
    #region Events for MainWindow
    public event Action? ModeSelected;
    public void SelectMode(DeploymentMode mode)
    {
        configuration.Mode = mode;

        ModeSelected?.Invoke();
    }
    #endregion
}
