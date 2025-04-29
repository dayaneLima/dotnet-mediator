using Mediator.Abstractions;

namespace Mediator.Sample.Console;

public record CreateUserRequest(string Username, string Password) : IRequest<string>;