using BSourceCore.Domain.Enums;

namespace BSourceCore.API.Contracts.Responses;

public record UserAuditResponse(
    Guid UserId,
    string Name);
