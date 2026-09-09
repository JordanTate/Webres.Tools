# WebRes Deployment Tool

A Windows desktop tool for deploying WebRes versions to an existing website and database. It supports a preview mode so deployments can be validated before making changes.

## Projects

- `Deployment.Core` contains UI-agnostic deployment operations, configuration models, providers, and version-specific deployment extensions.
- `Deployment.UI` is the WPF application that guides the operator through mode, version, server, website, review, and deployment.

Settings entered in the application are stored locally at `%LocalAppData%\WebResDeploymentTool\deployment.db`.

## Deployment workflow

1. Select **Preview** or **Deploy**, then choose the WebRes version, server, website, and database connection.
2. Review the selected configuration and run a preview first when possible.
3. Run the deployment. A non-preview deployment copies assemblies, replaces the `admin` directory, copies `DesktopModules` `.aspx` files, executes SQL scripts, and applies version-specific changes.

The selected version directory must provide the expected layout:

```text
<version>/
  build/bin/
  build/admin/
  build/DesktopModules/
  scripts/*.sql
```

SQL scripts are executed in filename order. Review and back up the target database before a non-preview deployment; scripts run with the configured connection's permissions.

## Extending versions

Add an `IVersionDeployment` implementation in `Deployment.Core/Deployment/VersionDeployment` for each supported version and register it with the UI application's dependency-injection container. The implementation receives the version and website paths and is responsible for that version's additional steps.

## Build

```powershell
dotnet build Webres.Tools.slnx
```
