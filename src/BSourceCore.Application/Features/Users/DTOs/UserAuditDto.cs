using BSourceCore.Domain.Enums;

namespace BSourceCore.Application.Features.Users.DTOs;

public record UserAuditDto(
    Guid UserId,
    string Name,
    string Email);
