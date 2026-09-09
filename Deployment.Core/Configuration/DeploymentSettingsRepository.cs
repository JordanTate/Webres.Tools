using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Deployment.Core.Configuration;

public sealed class DeploymentSettingsRepository : IDeploymentSettingsRepository
{
    private readonly string _connectionString;

    public DeploymentSettingsRepository()
    {
        string databasePath = GetDatabasePath();

        _connectionString =
            $"Data Source={databasePath}";
    }

    public async Task<DeploymentSettings?> GetAsync()
    {
        await using var connection
            = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        await EnsureTableExistsAsync(connection);
        await using var command = connection.CreateCommand();

        command.CommandText = """
            SELECT UpdateSource, WebServers, WebsiteRoot
            FROM DeploymentSettings
            WHERE Id = 1;
            """;

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        string updateSource = reader.GetString(0);
        string[] webServers =
            JsonSerializer.Deserialize<string[]>(reader.GetString(1))
            ?? [];
        string websiteRoot = reader.GetString(2);

        return new()
        {
            UpdateSource = updateSource,
            WebServers = webServers,
            WebsiteRoot = websiteRoot
        };
    }

    public async Task SaveAsync(DeploymentSettings settings)
    {
        await using var connection
            = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        await EnsureTableExistsAsync(connection);

        string webServers =
            JsonSerializer.Serialize(settings.WebServers);

        await using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO DeploymentSettings
                (Id, UpdateSource, WebServers, WebsiteRoot)
            VALUES
                (1, $updateSource, $webServers, $websiteRoot)
            ON CONFLICT(Id) DO UPDATE SET
                UpdateSource = excluded.UpdateSource,
                WebServers = excluded.WebServers,
                WebsiteRoot = excluded.WebsiteRoot;
            """;

        command.Parameters.AddWithValue(
            "$updateSource",
            settings.UpdateSource);

        command.Parameters.AddWithValue(
            "$webServers",
            webServers);

        command.Parameters.AddWithValue(
            "$websiteRoot",
            settings.WebsiteRoot);

        await command.ExecuteNonQueryAsync();
    }

    private static string GetDatabasePath()
    {
        string appDataPath = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
                "WebResDeploymentTool");

        Directory.CreateDirectory(appDataPath);

        return Path.Combine(appDataPath, "deployment.db");
    }

    private static async Task EnsureTableExistsAsync(
        SqliteConnection connection)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS DeploymentSettings
            (
                Id INTEGER NOT NULL PRIMARY KEY,
                UpdateSource TEXT NOT NULL,
                WebServers TEXT NOT NULL,
                WebsiteRoot TEXT NOT NULL
            );
            """;

        await command.ExecuteNonQueryAsync();
    }
}
