using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Website;

public sealed record WebsiteInfo(
    string Name,
    string Path);
