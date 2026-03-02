namespace BSourceCore.Application.Models.Requests;

public record TokenSubject(
    Guid UserId,
    string Name,
    string Email,
    Guid TenantId,
    IEnumerable<string> PermissionCodes);
