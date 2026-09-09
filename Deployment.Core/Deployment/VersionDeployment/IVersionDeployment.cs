using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Deployment.VersionDeployment;

public interface IVersionDeployment
{
    string Version { get; }

    Task ApplyAsync(
        string versionPath,
        string websitePath,
        bool isPreviewRun);
}
