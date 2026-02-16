namespace BSourceCore.Application.Features.Groups.DTOs;

public record GroupDto(
    Guid GroupId,
    Guid TenantId,
    string Name,
    string? Description,
    string Status,
    DateTimeOffset CreatedAt);
