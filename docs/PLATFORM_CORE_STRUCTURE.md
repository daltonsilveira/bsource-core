# PLATFORM_CORE_STRUCTURE

## 1. Objetivo

Este documento define a **materialização técnica** do Platform Core em código e API.

Ele especifica:

* Estrutura das entidades
* Relacionamentos
* Contratos de API
* Commands e Queries
* Estrutura de arquivos

---

## 2. Entidades do Domain

### 2.1 Tenant

```csharp
public class Tenant : AuditEntity
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public string? Description { get; private set; }
    public BaseStatus Status { get; private set; } = BaseStatus.Active;

    // Navegação
    public ICollection<User> Users { get; private set; }
    public ICollection<Group> Groups { get; private set; }
    public ICollection<Permission> Permissions { get; private set; }
}
```

### 2.2 User

```csharp
public class User : AuditEntity
{
    public Guid UserId { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public BaseStatus Status { get; private set; } = BaseStatus.Active;

    // Navegação
    public Tenant Tenant { get; private set; }
    public ICollection<UserGroup> UserGroups { get; private set; }
}
```

### 2.3 Group

```csharp
public class Group : AuditEntity
{
    public Guid GroupId { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public BaseStatus Status { get; private set; } = BaseStatus.Active;

    // Navegação
    public Tenant Tenant { get; private set; }
    public ICollection<UserGroup> UserGroups { get; private set; }
    public ICollection<GroupPermission> GroupPermissions { get; private set; }
}
```

### 2.4 Permission

```csharp
public class Permission : AuditEntity
{
    public Guid PermissionId { get; private set; }
    public Guid TenantId { get; private set; }
    public string Code { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public BaseStatus Status { get; private set; } = BaseStatus.Active;

    // Navegação
    public Tenant Tenant { get; private set; }
    public ICollection<GroupPermission> GroupPermissions { get; private set; }
}
```

### 2.5 UserGroup

```csharp
public class UserGroup : AuditEntity
{
    public Guid UserGroupId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid GroupId { get; private set; }

    // Navegação
    public User User { get; private set; }
    public Group Group { get; private set; }
}
```

### 2.6 GroupPermission

```csharp
public class GroupPermission : AuditEntity
{
    public Guid GroupPermissionId { get; private set; }
    public Guid GroupId { get; private set; }
    public Guid PermissionId { get; private set; }

    // Navegação
    public Group Group { get; private set; }
    public Permission Permission { get; private set; }
}
```

---

## 3. AuditEntity

Todas as entidades **DEVEM** herdar de AuditEntity:

```csharp
public abstract class AuditEntity
{
    public Guid? CreatedById { get; protected set; }
    public DateTimeOffset CreatedAt { get; protected set; }
    public Guid? UpdatedById { get; protected set; }
    public DateTimeOffset? UpdatedAt { get; protected set; }
}
```

---

## 3.1 Chave Primária

Conforme ARCHITECTURE_GUIDE seção 10:

* Toda entidade **DEVE** possuir uma chave única
* **NÃO** utilizar chaves compostas
* A chave **DEVE** seguir o padrão `NomeDaEntidadeId`

Exemplos:
* `Tenant` → `TenantId`
* `User` → `UserId`
* `UserGroup` → `UserGroupId`

---

## 3.1 Status

O campo `Status` **DEVE** ser declarado individualmente em cada entidade que o necessita:

* Permite variações específicas por entidade no futuro
* Entidades de associação (UserGroup, GroupPermission) **NÃO** possuem Status
* O valor padrão é `BaseStatus.Active`

---

## 4. Estrutura de Arquivos

### 4.1 Domain

```
/src/NomeProjeto.Domain
 └── /Entities
      ├── AuditEntity.cs
      ├── Tenant.cs
      ├── User.cs
      ├── Group.cs
      ├── Permission.cs
      ├── UserGroup.cs
      └── GroupPermission.cs
 └── /Enums
      └── BaseStatus.cs
```

### 4.2 Application

```
/src/NomeProjeto.Application
 └── /Tenants
      ├── /Commands
      │    ├── CreateTenant
      │    │    ├── CreateTenantCommand.cs
      │    │    ├── CreateTenantCommandHandler.cs
      │    │    └── CreateTenantCommandValidator.cs
      │    ├── UpdateTenant
      │    │    ├── UpdateTenantCommand.cs
      │    │    ├── UpdateTenantCommandHandler.cs
      │    │    └── UpdateTenantCommandValidator.cs
      │    └── DeleteTenant
      │         ├── DeleteTenantCommand.cs
      │         └── DeleteTenantCommandHandler.cs
      ├── /Queries
      │    ├── GetTenantById
      │    │    ├── GetTenantByIdQuery.cs
      │    │    └── GetTenantByIdQueryHandler.cs
      │    └── GetTenants
      │         ├── ListTenantsQuery.cs
      │         └── ListTenantsQueryHandler.cs
      └── /DTOs
           └── TenantDto.cs

 └── /Users
      ├── /Commands
      │    ├── CreateUser
      │    ├── UpdateUser
      │    └── DeleteUser
      ├── /Queries
      │    ├── GetUserById
      │    └── GetUsers
      └── /DTOs
           └── UserDto.cs

 └── /Groups
      ├── /Commands
      │    ├── CreateGroup
      │    ├── UpdateGroup
      │    ├── DeleteGroup
      │    ├── AddUserToGroup
      │    ├── RemoveUserFromGroup
      │    ├── AddPermissionToGroup
      │    └── RemovePermissionFromGroup
      ├── /Queries
      │    ├── GetGroupById
      │    └── GetGroups
      └── /DTOs
           └── GroupDto.cs

 └── /Permissions
      ├── /Commands
      │    ├── CreatePermission
      │    ├── UpdatePermission
      │    └── DeletePermission
      ├── /Queries
      │    ├── GetPermissionById
      │    └── GetPermissions
      └── /DTOs
           └── PermissionDto.cs

 └── /Auth
      ├── /Commands
      │    └── Login
      │         ├── LoginCommand.cs
      │         ├── LoginCommandHandler.cs
      │         └── LoginCommandValidator.cs
      └── /DTOs
           └── TokenDto.cs
```

### 4.3 API

```
/src/NomeProjeto.API
 └── /Controllers
      ├── TenantsController.cs
      ├── UsersController.cs
      ├── GroupsController.cs
      ├── PermissionsController.cs
      └── AuthController.cs
 └── /Requests
      ├── /Tenants
      │    ├── CreateTenantRequest.cs
      │    └── UpdateTenantRequest.cs
      ├── /Users
      │    ├── CreateUserRequest.cs
      │    └── UpdateUserRequest.cs
      ├── /Groups
      │    ├── CreateGroupRequest.cs
      │    ├── UpdateGroupRequest.cs
      │    ├── AddUserToGroupRequest.cs
      │    └── AddPermissionToGroupRequest.cs
      ├── /Permissions
      │    ├── CreatePermissionRequest.cs
      │    └── UpdatePermissionRequest.cs
      └── /Auth
           └── LoginRequest.cs
 └── /Responses
      ├── TenantResponse.cs
      ├── UserResponse.cs
      ├── GroupResponse.cs
      ├── PermissionResponse.cs
      └── TokenResponse.cs
```

### 4.4 Infrastructure

```
/src/NomeProjeto.Infrastructure
 └── /Persistence
      ├── /Configurations
      │    ├── TenantConfiguration.cs
      │    ├── UserConfiguration.cs
      │    ├── GroupConfiguration.cs
      │    ├── PermissionConfiguration.cs
      │    ├── UserGroupConfiguration.cs
      │    └── GroupPermissionConfiguration.cs
      ├── /Repositories
      │    ├── TenantRepository.cs
      │    ├── UserRepository.cs
      │    ├── GroupRepository.cs
      │    └── PermissionRepository.cs
      ├── WriteDbContext.cs
      └── ReadOnlyDbContext.cs
```

---

## 5. Contratos de API

### 5.1 Tenants

| Método | Rota | Descrição | Permissão |
|--------|------|-----------|-----------|
| POST | /api/tenants | Criar tenant | tenants.create |
| GET | /api/tenants | Listar tenants | tenants.read |
| GET | /api/tenants/{id} | Obter tenant | tenants.read |
| PUT | /api/tenants/{id} | Atualizar tenant | tenants.update |
| DELETE | /api/tenants/{id} | Remover tenant | tenants.delete |

### 5.2 Users

| Método | Rota | Descrição | Permissão |
|--------|------|-----------|-----------|
| POST | /api/users | Criar usuário | users.create |
| GET | /api/users | Listar usuários | users.read |
| GET | /api/users/{id} | Obter usuário | users.read |
| PUT | /api/users/{id} | Atualizar usuário | users.update |
| DELETE | /api/users/{id} | Remover usuário | users.delete |

### 5.3 Groups

| Método | Rota | Descrição | Permissão |
|--------|------|-----------|-----------|
| POST | /api/groups | Criar grupo | groups.create |
| GET | /api/groups | Listar grupos | groups.read |
| GET | /api/groups/{id} | Obter grupo | groups.read |
| PUT | /api/groups/{id} | Atualizar grupo | groups.update |
| DELETE | /api/groups/{id} | Remover grupo | groups.delete |
| POST | /api/groups/{id}/users | Adicionar usuário | groups.update |
| DELETE | /api/groups/{id}/users/{userId} | Remover usuário | groups.update |
| POST | /api/groups/{id}/permissions | Adicionar permissão | groups.update |
| DELETE | /api/groups/{id}/permissions/{permissionId} | Remover permissão | groups.update |

### 5.4 Permissions

| Método | Rota | Descrição | Permissão |
|--------|------|-----------|-----------|
| POST | /api/permissions | Criar permissão | permissions.create |
| GET | /api/permissions | Listar permissões | permissions.read |
| GET | /api/permissions/{id} | Obter permissão | permissions.read |
| PUT | /api/permissions/{id} | Atualizar permissão | permissions.update |
| DELETE | /api/permissions/{id} | Remover permissão | permissions.delete |

### 5.5 Auth

| Método | Rota | Descrição | Permissão |
|--------|------|-----------|-----------|
| POST | /api/auth/login | Autenticar | Público |

---

## 6. Exemplo de Controller

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Policy = "users.create")]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var command = new CreateUserCommand(
            request.TenantId,
            request.Name,
            request.Email,
            request.Password
        );

        var result = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetById),
            new { userId = result.UserId },
            new UserResponse(result.UserId, result.Name, result.Email)
        );
    }

    [HttpGet("{userId:guid}")]
    [Authorize(Policy = "users.read")]
    public async Task<IActionResult> GetById(Guid userId)
    {
        var query = new GetUserByIdQuery(userId);
        var result = await _mediator.Send(query);

        if (result is null)
            return NotFound();

        return Ok(new UserResponse(result.UserId, result.Name, result.Email));
    }
}
```

---

## 7. Exemplo de Command e Handler

### 7.1 Command

```csharp
public record CreateUserCommand(
    Guid TenantId,
    string Name,
    string Email,
    string Password
) : IRequest<UserDto>;
```

### 7.2 Validator

```csharp
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}
```

### 7.3 Handler

```csharp
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = new User(
            request.TenantId,
            request.Name,
            request.Email,
            passwordHash
        );

        await _userRepository.AddAsync(user, cancellationToken);

        return new UserDto(user.UserId, user.Name, user.Email);
    }
}
```

---

## 8. Dados Padrão

### 8.1 Permissões

O Platform Core **DEVE** criar as seguintes permissões no seed inicial:

```
all (esta permissão dará acesso a tudo no sistema. Isso deve ser tratado nas Policies)

tenants.create
tenants.read
tenants.update
tenants.delete

users.create
users.read
users.update
users.delete

groups.create
groups.read
groups.update
groups.delete

permissions.create
permissions.read
permissions.update
permissions.delete
```

### 8.2 Tenant

O Platform Core **DEVE** criar  no seed inicial 1 tenant.

### 8.3 Usuário, Grupo e Permissão

- O Platform Core **DEVE** criar no seed inicial 1 usuário padrão, com o login 'admin' e a senha '123456'.
- O Platform Core **DEVE** criar no seed inicial 1 grupo padrão, com o nome de 'Administrador'.
- O grupo 'Administrador' terá permissão 'all'.

---

## 9. Fluxo de Autorização

```
Request com JWT
       ↓
Extrair UserId do Token
       ↓
Buscar UserGroups do User
       ↓
Buscar GroupPermissions dos Groups
       ↓
Verificar se Permission requerida existe
       ↓
Autorizar ou Negar
```

---

## 10. Diagrama de Relacionamentos

```
┌─────────────┐
│   Tenant    │
└──────┬──────┘
       │
       │ 1:N
       ▼
┌─────────────┐      N:N      ┌─────────────┐
│    User     │◄─────────────►│    Group    │
└─────────────┘  (UserGroup)  └──────┬──────┘
                                     │
                                     │ N:N
                                     ▼
                              ┌─────────────┐
                              │ Permission  │
                              └─────────────┘
                            (GroupPermission)
```

---

## 11. Regras de Isolamento por Tenant

11.1 Todas as queries **DEVEM** filtrar por TenantId

11.2 O TenantId **DEVE** ser extraído do token JWT

11.3 Nenhum usuário pode acessar dados de outro Tenant

11.4 A filtragem **DEVE** ser aplicada automaticamente via Global Query Filter

```csharp
// No DbContext
modelBuilder.Entity<User>()
    .HasQueryFilter(u => u.TenantId == _tenantContext.TenantId);
```

---

## 12. Considerações Finais

Este documento define a estrutura técnica obrigatória para o Platform Core.

Qualquer desvio:

* Deve ser consciente
* Deve ser documentado
* Deve ser justificado

A implementação **DEVE** seguir rigorosamente as diretrizes do ARCHITECTURE_GUIDE.
