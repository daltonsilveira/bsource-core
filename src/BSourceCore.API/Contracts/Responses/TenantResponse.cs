using BSourceCore.Domain.Enums;

namespace BSourceCore.API.Contracts.Responses;

public record TenantResponse(
    Guid TenantId,
    string Name,
    string Slug,
    string Description,
    string Status,
    DateTimeOffset CreatedAt,
    UserAuditResponse? CreatedBy = null,
    DateTimeOffset? UpdatedAt = null,
    UserAuditResponse? UpdatedBy = null);
