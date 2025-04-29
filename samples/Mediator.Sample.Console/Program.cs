using Mediator.Abstractions;
using Mediator.Extensions;
using Mediator.Sample.Console;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddTransient<UserRepository>();

services.AddMediator(typeof(Program).Assembly);

var serviceProvider = services.BuildServiceProvider();

var mediator = serviceProvider.GetRequiredService<IMediator>();

var request = new CreateUserRequest("teste", "123456");

var result = await mediator.SendAsync(request);
Console.WriteLine($"[Console] Result: {result}");