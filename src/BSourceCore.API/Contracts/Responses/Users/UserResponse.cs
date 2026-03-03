using BSourceCore.Domain.Enums;

namespace BSourceCore.API.Contracts.Responses;

public record UserResponse(
    Guid UserId,
    string Name,
    string Email,
    BaseStatus Status,
    DateTimeOffset CreatedAt,
    UserAuditResponse? CreatedBy,
    DateTimeOffset? UpdatedAt,
    UserAuditResponse? UpdatedBy);


