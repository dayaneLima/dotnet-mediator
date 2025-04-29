
# 🧩 Simple Mediator for .NET

Um Mediator leve e simples para aplicações .NET, inspirado no padrão [Mediator](https://refactoring.guru/design-patterns/mediator), que ajuda a desacoplar a lógica de comunicação entre objetos.

## ✨ Funcionalidades

- Envio de requisições (`SendAsync<TResponse>`)
- Publicação de notificações para múltiplos handlers (`PublishAsync<TNotification>`)
- Registro automático de handlers via assembly scanning
- Compatível com .NET 9 e `Microsoft.Extensions.DependencyInjection`

---

## 📦 Instalação

Adicione o projeto como referência no seu projeto principal, ou transforme em um pacote NuGet (opcional).

---

## 🛠️ Configuração

No `Program.cs` ou `Startup.cs`, registre o Mediator e os handlers:

```csharp
using Mediator.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediator(typeof(Program).Assembly);
```

---

## ✅ Como Usar

### 1. Defina uma requisição e seu handler:

```csharp
public class GetUserQuery : IRequest<UserDto>
{
    public int Id { get; init; }
}

public class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
{
    public Task<UserDto> HandleAsync(GetUserQuery request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new UserDto { Id = request.Id, Name = "Alice" });
    }
}
```

### 2. Envie a requisição:

```csharp
var response = await mediator.SendAsync(new GetUserQuery { Id = 1 });
```

---

### 3. Defina uma notificação e seus handlers:

```csharp
public class UserCreatedNotification : INotification
{
    public string Email { get; init; }
}

public class SendWelcomeEmailHandler : INotificationHandler<UserCreatedNotification>
{
    public Task HandleAsync(UserCreatedNotification notification, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Enviando e-mail para: {notification.Email}");
        return Task.CompletedTask;
    }
}
```

### 4. Publique a notificação:

```csharp
await mediator.PublishAsync(new UserCreatedNotification { Email = "user@example.com" });
```

---

## 📁 Estrutura

```
Mediator/
│
├── Abstractions/         # Interfaces: IRequest, INotification, Handlers, IMediator
├── Extensions/           # Extension method para registrar os handlers
├── Mediator.cs           # Implementação principal do IMediator
└── README.md             # Este arquivo
```

---

## 📜 Licença

Este projeto está licenciado sob a [MIT License](LICENSE).