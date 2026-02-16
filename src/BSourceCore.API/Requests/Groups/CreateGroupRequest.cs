namespace BSourceCore.API.Requests.Groups;

public record CreateGroupRequest(
    Guid TenantId,
    string Name,
    string? Description);
