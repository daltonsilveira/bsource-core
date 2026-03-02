namespace BSourceCore.API.Contracts.Requests.Groups;

public record CreateGroupRequest(
    Guid TenantId,
    string Name,
    string? Description);
