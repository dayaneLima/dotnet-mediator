using Mediator.Abstractions;namespace Mediator.Sample.Console;

public class SmsUserHandler : INotificationHandler<CreateUserEvent>
{
    public async Task HandleAsync(CreateUserEvent notification, CancellationToken cancellationToken = default)
    {
        System.Console.WriteLine($"[SmsUserHandler] sending sms for user {notification.Username}...");
    }
}
