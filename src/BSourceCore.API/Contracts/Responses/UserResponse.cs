namespace BSourceCore.API.Contracts.Responses;

public record UserResponse(
    Guid UserId,
    Guid TenantId,
    string Name,
    string Email,
    string Status,
    DateTimeOffset CreatedAt);
