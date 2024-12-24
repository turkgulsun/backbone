using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace Core.Infrastructure.HealthChecks;

public class NpgsqlServerHealthCheck : IHealthCheck
{
    private readonly string _connectionString;

    private readonly string _sql;

    public NpgsqlServerHealthCheck(string connectionString, string sql)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _sql = sql ?? throw new ArgumentNullException(nameof(sql));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync(cancellationToken);
            
            using (NpgsqlCommand command = connection.CreateCommand())
            {
                command.CommandText = _sql;
                await command.ExecuteScalarAsync(cancellationToken);
            }

            return HealthCheckResult.Healthy();
        }
        catch (Exception exception)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, null, exception);
        }
    }
}