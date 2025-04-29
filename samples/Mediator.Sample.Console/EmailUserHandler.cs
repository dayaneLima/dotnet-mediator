using Mediator.Abstractions;

namespace Mediator.Sample.Console;

public class EmailUserHandler : INotificationHandler<CreateUserEvent>
{
    public async Task HandleAsync(CreateUserEvent notification, CancellationToken cancellationToken = default)
    {
        System.Console.WriteLine($"[EmailUserHandler] sending email for user {notification.Username}...");
    }
}