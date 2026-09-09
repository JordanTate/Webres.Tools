using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Deployment.Core.Connection;

public sealed class ConnectionValidator : IConnectionValidator
{
    public async Task<ConnectionValidationResult> ValidateAsync(
        ConnectionInfo connection,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var builder = new SqlConnectionStringBuilder(
                connection.ConnectionString)
            {
                TrustServerCertificate = true
            };

            await using SqlConnection sqlConnection = new(builder.ConnectionString);
            await sqlConnection.OpenAsync(cancellationToken);
            await using SqlCommand command = new("SELECT 1", sqlConnection);
            await command.ExecuteScalarAsync(cancellationToken);

            return new()
            {
                Success = true,
                Duration = stopwatch.Elapsed,
            };
        }
        catch (SqlException ex)
        {
            return new()
            {
                Success = false,
                ErrorMessage = ex.Message,
                Duration = stopwatch.Elapsed,
            };
        }
    }
}
