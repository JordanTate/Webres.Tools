using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Version;

public sealed record VersionInfo(
    string Value,
    string Path);
