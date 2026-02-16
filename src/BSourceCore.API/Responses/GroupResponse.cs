namespace BSourceCore.API.Responses;

public record GroupResponse(
    Guid GroupId,
    Guid TenantId,
    string Name,
    string? Description,
    string Status,
    DateTimeOffset CreatedAt);
