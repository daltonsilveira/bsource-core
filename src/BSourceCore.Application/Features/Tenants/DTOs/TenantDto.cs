using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Domain.Enums;

namespace BSourceCore.Application.Features.Tenants.DTOs;

public record TenantDto(
    Guid TenantId,
    string Name,
    string Slug,
    string Description,
    BaseStatus Status,
    DateTimeOffset CreatedAt,
    UserAuditDto? CreatedBy = null,
    DateTimeOffset? UpdatedAt = null,
    UserAuditDto? UpdatedBy = null);
