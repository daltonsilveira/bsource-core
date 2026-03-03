namespace BSourceCore.API.Contracts.Responses;

public record CurrentUserResponse(
    Guid UserId,
    string Email,
    string Name,
    Guid TenantId,
    List<string> PermissionCodes);
