using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Core.Infrastructure.HealthChecks;

public static class HealthCheckBuilderExtensions
{
    public static IHealthChecksBuilder AddHttp(
        this IHealthChecksBuilder builder,
        string uri,
        string name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string> tags = default,
        TimeSpan? timeout = default)
    {
        return builder.Add(new HealthCheckRegistration(
            name,
            new HttpHealthCheck(uri),
            failureStatus,
            tags,
            timeout));
    }

    public static IHealthChecksBuilder AddNpgsqlServer(
        this IHealthChecksBuilder builder,
        string connectionString,
        string healthQuery = default,
        string name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string> tags = default,
        TimeSpan? timeout = default)
    {
        return builder.Add(new HealthCheckRegistration(
            name,
            new NpgsqlServerHealthCheck(connectionString, healthQuery),
            failureStatus,
            tags,
            timeout));
    }

    public static IHealthChecksBuilder AddKafka(
        this IHealthChecksBuilder builder,
        string bootstrapServers,
        string topic,
        string name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string> tags = default,
        TimeSpan? timeout = default)
    {
        return builder.Add(new HealthCheckRegistration(
            name,
            new KafkaHealthCheck(bootstrapServers: bootstrapServers, topic: topic),
            failureStatus,
            tags,
            timeout));
    }
}