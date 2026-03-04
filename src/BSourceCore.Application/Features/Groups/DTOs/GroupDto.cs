using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Domain.Enums;

namespace BSourceCore.Application.Features.Groups.DTOs;

public record GroupDto(
    Guid GroupId,
    Guid TenantId,
    string Name,
    string Description,
    BaseStatus Status,
    DateTimeOffset CreatedAt,
    UserAuditDto? CreatedBy = null,
    DateTimeOffset? UpdatedAt = null,
    UserAuditDto? UpdatedBy = null);
