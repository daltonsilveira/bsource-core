namespace BSourceCore.API.Contracts.Requests.Tenants;

public record UpdateTenantRequest(
    string Name,
    string Slug,
    string? Description);
