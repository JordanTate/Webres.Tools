using Deployment.Core.Configuration;
using Deployment.Core.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Website;

public interface IWebsiteProvider
{
    /// <summary>
    /// Returns a list of websites by directories containing 'Web.config' files on a specific server path.
    /// </summary>
    /// <param name="server">
    /// The server information that will be used to perform the search.
    /// </param>
    /// <returns>
    /// A read-only list of website information.
    /// </returns>
    Task<IReadOnlyList<WebsiteInfo>> GetWebsitesByServer(
        ServerInfo server);
}
