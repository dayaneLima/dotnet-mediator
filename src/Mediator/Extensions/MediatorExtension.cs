using Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Mediator.Extensions;

public static class MediatorExtension
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddSingleton<IMediator, Mediator>();

        RegisterHandlers(services, assemblies, typeof(IRequestHandler<,>));
        RegisterHandlers(services, assemblies, typeof(INotificationHandler<>));

        return services;
    }

    private static void RegisterHandlers(IServiceCollection services, Assembly[] assemblies, Type handlerInterfaceType)
    {
        foreach (var assembly in assemblies)
        {
            var handlers = assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
                .Where(ti => ti.Interface.IsGenericType && ti.Interface.GetGenericTypeDefinition() == handlerInterfaceType);

            foreach (var handler in handlers)
                services.AddTransient(handler.Interface, handler.Type);
        }
    }
}