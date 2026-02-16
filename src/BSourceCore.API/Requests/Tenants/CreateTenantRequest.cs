namespace BSourceCore.API.Requests.Tenants;

public record CreateTenantRequest(
    string Name,
    string Slug,
    string? Description);
