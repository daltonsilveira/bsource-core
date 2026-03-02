namespace BSourceCore.API.Contracts.Responses;

public record GroupResponse(
    Guid GroupId,
    Guid TenantId,
    string Name,
    string? Description,
    string Status,
    DateTimeOffset CreatedAt);
