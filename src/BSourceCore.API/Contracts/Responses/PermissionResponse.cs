namespace BSourceCore.API.Contracts.Responses;

public record PermissionResponse(
    Guid PermissionId,
    string Code,
    string Name,
    string? Description,
    string Status,
    DateTimeOffset CreatedAt);
