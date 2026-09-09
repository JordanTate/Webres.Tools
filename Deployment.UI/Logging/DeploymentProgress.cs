using Deployment.Core.Deployment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.UI.Logging;

/// <summary>
/// Reports deployment progress.
/// </summary>
public sealed class DeploymentProgress : IDeploymentProgress
{
    public event Action<string, int>? ProgressReported;

    public void Report(string message, int percentage)
    {
        ProgressReported?.Invoke(message, percentage);
    }
}
