using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Deployment;

public sealed class DeploymentResult
{
    public bool Success { get; init; }
    public bool WasPreviewRun { get; init; }
    public string? ErrorMessage { get; init; }
    public bool AssembliesOperationSuccessful { get; init; }
    public bool EssentialFilesOperationSuccessful { get; init; }
    public bool SqlScriptsOperationSuccessful { get; init; }
    public bool VersionChangesOperationSuccessful { get; init; }
}
