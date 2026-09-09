using Deployment.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.ViewModels;

internal class MainWindowModel
{
    public DeploymentModeViewModel DeploymentMode { get; }

    public MainWindowModel()
    {
        DeploymentMode = new DeploymentModeViewModel();
    }
}
