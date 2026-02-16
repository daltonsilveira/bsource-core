namespace BSourceCore.Application.Features.Users.DTOs;

public record UserDto(
    Guid UserId,
    Guid TenantId,
    string Name,
    string Email,
    string Status,
    DateTimeOffset CreatedAt);
