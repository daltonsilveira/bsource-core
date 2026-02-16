namespace BSourceCore.Application.Features.Permissions.DTOs;

public record PermissionDto(
    Guid PermissionId,
    string Code,
    string Name,
    string? Description,
    string Status,
    DateTimeOffset CreatedAt);
