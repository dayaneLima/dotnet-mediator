using Mediator.Abstractions;

namespace Mediator.Sample.Console;

public class CreateUserHandler(UserRepository UserRepository, IMediator Mediator) : IRequestHandler<CreateUserRequest, string>
{
    public async Task<string> HandleAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        System.Console.WriteLine($"[CreateUserHandler] Creating user {request.Username}...");

        UserRepository.Save();
        await Mediator.PublishAsync(new CreateUserEvent(request.Username), cancellationToken);

        return $"{request.Username} created";
    }
}