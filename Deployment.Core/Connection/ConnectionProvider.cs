using Deployment.Core.Website;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Xml.Linq;

namespace Deployment.Core.Connection;

public sealed class ConnectionProvider : IConnectionProvider
{
    private const string FILE_CONNECTION_STRINGS_CONFIG = "ConnectionStrings.Config";
    private const string FILE_WEB_CONFIG = "Web.Config";
    public ConnectionInfo? GetConnection(WebsiteInfo website)
    {
        string? encodedConnectionString = null;

        string connectionStringsConfig = Path.Combine(
            website.Path, FILE_CONNECTION_STRINGS_CONFIG);

        string webConfig = Path.Combine(
            website.Path, FILE_WEB_CONFIG);

        if (File.Exists(connectionStringsConfig))
        {
            encodedConnectionString = ReadConnectionString(connectionStringsConfig);
        }
        else if (File.Exists(webConfig))
        {
            encodedConnectionString = ReadConnectionString(webConfig);
        }

        if (string.IsNullOrWhiteSpace(encodedConnectionString))
            return null;

        return ParseConnectionString(encodedConnectionString);
    }

    private static string? ReadConnectionString(string configPath)
    {
        XDocument document = XDocument.Load(configPath);

        string? connectionString = document
            .Descendants("add")
            .FirstOrDefault(x =>
                string.Equals(
                    (string?)x.Attribute("name"),
                    "ConnectionString",
                    StringComparison.OrdinalIgnoreCase))
            ?.Attribute("connectionString")
            ?.Value;

        return connectionString;
    }

    private static ConnectionInfo? ParseConnectionString(string encodedString)
    {
        try
        {
            string? decoded = Encoding.UTF8.GetString(
                Convert.FromBase64String(encodedString));

            SqlConnectionStringBuilder builder = new(decoded);

            return new(
                builder.ConnectionString,
                builder.DataSource,
                builder.InitialCatalog,
                builder.IntegratedSecurity
                    ? "Windows"
                    : "SQL");
        }
        catch (FormatException)
        {
            return null;
        }
    }
}
