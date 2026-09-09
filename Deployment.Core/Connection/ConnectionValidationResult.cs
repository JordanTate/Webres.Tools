using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Connection;

public sealed class ConnectionValidationResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public TimeSpan? Duration { get; init; }
}
