using Deployment.Core.Configuration;
using Deployment.Core.Deployment.VersionDeployment;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Deployment;

public sealed class DeploymentService(
    IVersionDeploymentProvider versionDeploymentProvider,
    IDeploymentProgress progress) : IDeploymentService
{
    public async Task<DeploymentResult> DeployAsync(DeploymentConfiguration configuration)
    {
        if (configuration.Version is null)
            throw new InvalidOperationException("No version information found in configuration.");

        if (configuration.Website is null)
            throw new InvalidOperationException("No version information found in configuration.");

        if (configuration.Connection is null)
            throw new InvalidOperationException("No connection information found in configuration.");


        progress.Report("Starting deployment...", 0);

        bool isPreviewRun = configuration.Mode == DeploymentMode.Preview;
        bool assembliesDeployed = false;
        bool essentialFilesDeployed = false;
        bool sqlScriptsDeployed = false;
        bool versionChangesDeployed = false;

        try
        {
            progress.Report("Deployment assemblies...", 20);

            assembliesDeployed = DeployAssemblies(
                configuration.Version.Path,
                configuration.Website.Path,
                isPreviewRun);

            progress.Report("Deploying essential files...", 40);

            essentialFilesDeployed = DeployEssentialFiles(
                configuration.Version.Path,
                configuration.Website.Path,
                isPreviewRun);

            progress.Report("Running SQL scripts...", 60);

            sqlScriptsDeployed = await DeploySqlScripts(
                configuration.Version.Path,
                configuration.Connection.ConnectionString,
                isPreviewRun);

            progress.Report("Applying version-specific changes...", 80);

            IVersionDeployment? versionDeployment =
                versionDeploymentProvider.Get(
                    configuration.Version.Value);

            if (versionDeployment is null)
                throw new InvalidOperationException($"The 'IVersionDeployment' does not exist for version: {configuration.Version.Value}");

            await versionDeployment.ApplyAsync(
                configuration.Version.Path,
                configuration.Website.Path,
                isPreviewRun);

            versionChangesDeployed = true;

            progress.Report("Deployment completed.", 100);

            return new DeploymentResult
            {
                Success = true,
                WasPreviewRun = isPreviewRun,
                AssembliesOperationSuccessful = assembliesDeployed,
                EssentialFilesOperationSuccessful = essentialFilesDeployed,
                SqlScriptsOperationSuccessful = sqlScriptsDeployed,
                VersionChangesOperationSuccessful = versionChangesDeployed
            };
        }
        catch (Exception ex)
        {
            return new DeploymentResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                AssembliesOperationSuccessful = assembliesDeployed,
                EssentialFilesOperationSuccessful = essentialFilesDeployed,
                SqlScriptsOperationSuccessful = sqlScriptsDeployed,
                VersionChangesOperationSuccessful = false
            };
        }

        throw new NotImplementedException();
    }

    private bool DeployAssemblies(
        string versionPath,
        string websitePath,
        bool isPreviewRun)
    {
        string sourceBin = Path.Combine(versionPath, "build", "bin");
        string targetBin = Path.Combine(websitePath, "bin");

        EnsureDirectoryExists(sourceBin, "Source 'bin'");
        EnsureDirectoryExists(targetBin, "Target 'bin'");

        if (isPreviewRun)
            return true;

        CopyFiles(sourceBin, targetBin);

        return true;
    }

    private bool DeployEssentialFiles(
        string versionPath,
        string websitePath,
        bool isPreviewRun)
    {
        string sourceAdmin = Path.Combine(versionPath, "build", "admin");
        string targetAdmin = Path.Combine(websitePath, "admin");

        string sourceDesktopModules = Path.Combine(versionPath, "build", "DesktopModules");
        string targetDesktopModules = Path.Combine(websitePath, "DesktopModules");

        EnsureDirectoryExists(sourceAdmin, "Source 'Admin'");
        EnsureDirectoryExists(sourceDesktopModules, "Source 'DesktopModules'");
        EnsureDirectoryExists(targetAdmin, "Target 'Admin'");
        EnsureDirectoryExists(targetDesktopModules, "Target 'DesktopModules'");

        if (isPreviewRun)
            return true;

        Directory.Delete(targetAdmin, recursive: true);
        CopyDirectory(sourceAdmin, targetAdmin);
        CopyFiles(
            sourceDesktopModules,
            targetDesktopModules,
            "*.aspx");

        return true;
    }

    private async Task<bool> DeploySqlScripts(
        string versionPath,
        string connectionString,
        bool isPreviewRun)
    {
        string scriptsPath = Path.Combine(versionPath, "scripts");
        EnsureDirectoryExists(scriptsPath, "Source 'scripts'");

        string[] scripts = [.. Directory
            .EnumerateFiles(scriptsPath, "*.sql", SearchOption.TopDirectoryOnly)
            .OrderBy(Path.GetFileName)];

        if (isPreviewRun)
            return true;

        SqlConnectionStringBuilder builder = new (connectionString)
        {
            TrustServerCertificate = true
        };

        await using SqlConnection connection = new (builder.ConnectionString);
        await connection.OpenAsync();

        foreach (string script in scripts)
        {
            string sql = await File.ReadAllTextAsync(script);

            using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }

        return true;
    }

    #region Helper Methods
    private static void CopyDirectory(
        string sourceDirectory,
        string targetDirectory)
    {
        Directory.CreateDirectory(targetDirectory);

        foreach (string sourceFile in Directory.EnumerateFiles(sourceDirectory))
        {
            string fileName = Path.GetFileName(sourceFile);
            string targetFile = Path.Combine(targetDirectory, fileName);

            File.Copy(sourceFile, targetFile, overwrite: true);
        }

        foreach (string sourceSubdirectory in Directory.EnumerateDirectories(sourceDirectory))
        {
            string directoryName = Path.GetFileName(sourceSubdirectory);
            string targetSubdirectory = Path.Combine(targetDirectory, directoryName);

            CopyDirectory(sourceSubdirectory, targetSubdirectory);
        }
    }

    private static void CopyFiles(
        string sourceDirectory,
        string targetDirectory,
        string searchPattern = "*",
        SearchOption searchOption = SearchOption.AllDirectories)
    {
        foreach (string sourceFile in Directory.EnumerateFiles(
            sourceDirectory,
            searchPattern,
            searchOption))
        {
            string relativePath = Path.GetRelativePath(
                sourceDirectory,
                sourceFile);

            string targetFile = Path.Combine(
                targetDirectory,
                relativePath);

            string? targetDirectoryPath =
                Path.GetDirectoryName(targetFile);

            if (targetDirectoryPath is not null)
                Directory.CreateDirectory(targetDirectoryPath);

            File.Copy(
                sourceFile,
                targetFile,
                overwrite: true);
        }
    }

    private static void EnsureDirectoryExists(
        string path,
        string description)
    {
        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException(
                $"{description} directory was not found: {path}");
    }
    #endregion
}
