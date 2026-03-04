using BSourceCore.Domain.Enums;

namespace BSourceCore.API.Contracts.Responses;

public record UserResponse(
    Guid UserId,
    string Name,
    string Email,
    string Status,
    DateTimeOffset CreatedAt,
    UserAuditResponse? CreatedBy = null,
    DateTimeOffset? UpdatedAt = null,
    UserAuditResponse? UpdatedBy = null);


