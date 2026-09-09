using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Deployment;

public interface IDeploymentProgress
{
    /// <summary>
    /// Occurs when deployment progress is reported.
    /// </summary>
    event Action<string, int>? ProgressReported;

    /// <summary>
    /// Reports a deployment progress update.
    /// </summary>
    /// <param name="message">
    /// A message describing the current deployment activity.
    /// </param>
    /// <param name="percentage">
    /// The percentage of the deployment that has been completed (0-100).
    /// </param>
    void Report(string message, int percentage);
}
