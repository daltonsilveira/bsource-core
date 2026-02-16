namespace BSourceCore.API.Responses;

public record TenantResponse(
    Guid TenantId,
    string Name,
    string Slug,
    string? Description,
    string Status,
    DateTimeOffset CreatedAt);
