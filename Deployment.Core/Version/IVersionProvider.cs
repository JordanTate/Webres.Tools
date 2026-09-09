using Deployment.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Version;

public interface IVersionProvider
{
    /// <summary>
    /// Gets a list of available update versions.
    /// </summary>
    /// <returns>
    /// A read-only list of version information.
    /// </returns>
    Task<IReadOnlyList<VersionInfo>> GetVersionsAsync();
}
