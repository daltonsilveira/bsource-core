namespace BSourceCore.Application.Features.Tenants.Commands.CreateTenant;

public record CreateTenantResult(
    Guid TenantId,
    string Name,
    string Slug);
