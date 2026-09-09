using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Connection;

public sealed record ConnectionInfo(
    string ConnectionString,
    string ServerName,
    string DatabaseName,
    string AuthenticationType);
