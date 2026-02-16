namespace BSourceCore.API.Responses;

public record PermissionResponse(
    Guid PermissionId,
    string Code,
    string Name,
    string? Description,
    string Status,
    DateTimeOffset CreatedAt);
