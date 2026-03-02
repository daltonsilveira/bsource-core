namespace BSourceCore.API.Contracts.Requests.Tenants;

public record CreateTenantRequest(
    string Name,
    string Slug,
    string? Description);
