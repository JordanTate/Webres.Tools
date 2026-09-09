using Deployment.Core.Website;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Connection;

public interface IConnectionProvider
{
    /// <summary>
    /// Retrieves the connection information for the specified website.
    /// </summary>
    /// <param name="website">
    /// The website for which to retrieve connection information.
    /// </param>
    /// <returns>
    /// The connection information for the specified website, or <see langword="null"/>
    /// if no connection information is available.
    /// </returns>
    ConnectionInfo? GetConnection(WebsiteInfo website);
}
