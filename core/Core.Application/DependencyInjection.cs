using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application;

public static class DependencyInjection
{
    public static void AddCoreApplication(this IServiceCollection services, Assembly executingAssembly)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddMediatR(executingAssembly);
    }
}