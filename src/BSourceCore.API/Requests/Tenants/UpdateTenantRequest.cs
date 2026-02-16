namespace BSourceCore.API.Requests.Tenants;

public record UpdateTenantRequest(
    string Name,
    string Slug,
    string? Description);
