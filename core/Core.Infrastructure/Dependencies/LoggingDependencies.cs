using Core.Application.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Dependencies;

public static class LoggingDependencies
{
    public static IServiceCollection AddMediatrPipelineBehaviourLoggingDependency(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }
}