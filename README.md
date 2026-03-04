# BSourceCore - Plataforma de Gestão BSource

Backend base da plataforma BSource, desenvolvido em ASP.NET Core .NET 8 seguindo Clean Architecture.

## 🏗️ Arquitetura

O projeto segue rigorosamente as diretrizes definidas em:
- [ARCHITECTURE_GUIDE.md](docs/ARCHITECTURE_GUIDE.md)
- [PLATFORM_CORE_GUIDE.md](docs/PLATFORM_CORE_GUIDE.md)
- [PLATFORM_CORE_STRUCTURE.md](docs/PLATFORM_CORE_STRUCTURE.md)

### Estrutura de Projetos

```
src/
├── BSourceCore.API              # Camada de apresentação (Controllers, Requests, Responses)
├── BSourceCore.Application      # Casos de uso, Commands, Queries, Handlers, Validators
├── BSourceCore.Domain           # Entidades, Enums, Regras de negócio
├── BSourceCore.Infrastructure   # Persistência, DbContext, Repositories, Serviços externos
└── BSourceCore.Shared           # Abstrações compartilhadas, Result Pattern
```

### Fluxo de Dependências

```
API → Application → Domain
API → Application → Infrastructure (via abstrações)
```

## 🚀 Como Executar

### Pré-requisitos

- .NET 8 SDK ou superior
- PostgreSQL 14+
- Docker (opcional)

### Configuração do Banco de Dados

1. Criar o banco de dados PostgreSQL:
```sql
CREATE DATABASE BSourceCore_dev;
```

2. Atualizar a connection string em `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=BSourceCore_dev;Username=postgres;Password=postgres"
  }
}
```

### Executar Migrations

As migrations existentes (incluindo SeedData) são aplicadas automaticamente ao iniciar a API.
Use os comandos abaixo apenas para criar novas migrations quando necessário.

```bash
cd src/BSourceCore.API
dotnet ef migrations add InitialCreate --project ../BSourceCore.Infrastructure
dotnet ef database update --project ../BSourceCore.Infrastructure
```

### Executar o Projeto

```bash
cd src/BSourceCore.API
dotnet run
```

A API estará disponível em:
- **Swagger UI**: https://localhost:5001 (ou http://localhost:5000)
- **API Base**: https://localhost:5001/api/v1

## 📁 Estrutura Detalhada

### Domain Layer (`BSourceCore.Domain`)

```
Domain/
├── Entities/
│   ├── AuditEntity.cs          # Classe base com campos de auditoria
│   ├── Tenant.cs               # Entidade de tenant (multi-tenancy)
│   ├── User.cs                 # Entidade de usuário
│   ├── Group.cs                # Grupos de usuários
│   ├── Permission.cs           # Permissões
│   ├── UserGroup.cs            # Relação N:N User-Group
│   └── GroupPermission.cs      # Relação N:N Group-Permission
└── Enums/
    └── BaseStatus.cs           # Status base (Active, Inactive, Pending, Deleted)
```

### Application Layer (`BSourceCore.Application`)

```
Application/
├── Abstractions/               # Interfaces de repositórios
├── Behaviors/
│   └── ValidationBehavior.cs   # Pipeline de validação MediatR
├── Tenants/
│   ├── Commands/
│   │   └── CreateTenant/       # Command, Handler, Validator
│   ├── Queries/
│   │   ├── GetTenantById/
│   │   └── GetTenants/
│   └── DTOs/
│       └── TenantDto.cs
└── DependencyInjection.cs
```

### Infrastructure Layer (`BSourceCore.Infrastructure`)

```
Infrastructure/
├── Persistence/
│   ├── Configurations/         # EF Core Configurations
│   ├── Repositories/           # Implementação dos repositórios
│   ├── WriteDbContext.cs       # DbContext de escrita
│   └── ReadOnlyDbContext.cs    # DbContext de leitura (NoTracking)
├── Services/
│   └── PasswordHasher.cs       # Serviço de hash de senhas
└── DependencyInjection.cs
```

### API Layer (`BSourceCore.API`)

```
API/
├── Controllers/
│   └── TenantsController.cs
├── Requests/
│   └── Tenants/
├── Responses/
│   ├── ApiResponse.cs          # Resposta padronizada
│   └── TenantResponse.cs
├── Middleware/
│   └── GlobalExceptionHandler.cs
└── Program.cs
```

## 🔌 Endpoints

### Tenants

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/v1/tenants` | Criar tenant |
| GET | `/api/v1/tenants` | Listar tenants |
| GET | `/api/v1/tenants/{id}` | Obter tenant por ID |

## 🧪 Testando a API

### Criar um Tenant

```bash
curl -X POST https://localhost:5001/api/v1/tenants \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Empresa Teste",
    "slug": "empresa-teste",
    "description": "Descrição da empresa"
  }'
```

### Listar Tenants

```bash
curl https://localhost:5001/api/v1/tenants
```

## 🔧 Como Estender

### Adicionar Nova Entidade

1. Criar entidade em `Domain/Entities`:
```csharp
public class MinhaEntidade : AuditEntity
{
    public Guid MinhaEntidadeId { get; private set; }
    // ...
}
```

2. Criar Configuration em `Infrastructure/Persistence/Configurations`

3. Adicionar DbSet nos DbContexts

4. Criar Commands/Queries em `Application`

5. Criar Controller, Requests e Responses em `API`

### Adicionar Novo Caso de Uso

1. Criar Command/Query em `Application/{Feature}/Commands` ou `Queries`:
```csharp
public record MeuCommand(...) : IRequest<MeuResultDto>;
```

2. Criar Validator:
```csharp
public class MeuCommandValidator : AbstractValidator<MeuCommand> { }
```

3. Criar Handler:
```csharp
public class MeuCommandHandler : IRequestHandler<MeuCommand, MeuResultDto> { }
```

## 📋 Padrões Obrigatórios

- **Comunicação**: Toda comunicação API → Application via MediatR
- **Validação**: FluentValidation na camada Application
- **DTOs**: API usa Request/Response, Application usa DTOs internos
- **Auditoria**: Todas as entidades herdam de `AuditEntity`
- **Status**: Usar `BaseStatus` enum para status de entidades
- **Chaves**: Padrão `NomeDaEntidadeId` (ex: `TenantId`, `UserId`)

## 🔐 Autenticação (Preparado)

O projeto está preparado para autenticação JWT:
- Configuração em `appsettings.json`
- Middleware de autorização configurado
- Swagger configurado com suporte a Bearer token

Para implementar completamente:
1. Criar `AuthController` com endpoint de login
2. Implementar `TokenService` para geração de tokens
3. Descomentar `[Authorize]` nos controllers

## 📝 Licença

Projeto privado - BSource Consultoria
