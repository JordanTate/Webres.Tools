using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Server;

public sealed record ServerInfo(
    string Name,
    string Path);
