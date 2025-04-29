using Mediator.Abstractions;

namespace Mediator.Sample.Console;

public record CreateUserEvent(string Username) : INotification;