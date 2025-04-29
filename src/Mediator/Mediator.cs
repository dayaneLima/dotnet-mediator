using Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Mediator;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

        var handler = GetRequiredService(handlerType, request.GetType().Name);
        var method = GetRequiredMethod(handlerType, "HandleAsync");

        if (method.Invoke(handler, [request, cancellationToken]) is not Task<TResponse> task)
            throw new InvalidOperationException(
                $"Handler {handler.GetType().Name} returned null or an incompatible type.");

        return await task;
    }

    public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());

        var handlers = GetRequiredServices(handlerType, notification.GetType().Name);
        var method = GetRequiredMethod(handlerType, "HandleAsync");

        foreach (var handler in handlers)
        {
            var result = method.Invoke(handler, [notification, cancellationToken]);
            if (result is not Task task)
                throw new InvalidOperationException(
                    $"Handler {handler!.GetType().Name} returned null or an incompatible type.");

            await task;
        }
    }

    private object GetRequiredService(Type type, string name)
    {
        var service = serviceProvider.GetService(type);
        if (service == null)
            throw new InvalidOperationException($"Handler not found for {name}");
        return service;
    }

    private IEnumerable<object> GetRequiredServices(Type type, string name)
    {
        var services = serviceProvider.GetServices(type);
        if (services == null || !services.Any())
            throw new InvalidOperationException($"No handlers found for notification type {name}");
        return services!;
    }

    private static MethodInfo GetRequiredMethod(Type handlerType, string methodName)
    {
        var method = handlerType.GetMethod(methodName);
        if (method == null)
            throw new InvalidOperationException($"{methodName} method not found in {handlerType.Name}");
        return method;
    }
}