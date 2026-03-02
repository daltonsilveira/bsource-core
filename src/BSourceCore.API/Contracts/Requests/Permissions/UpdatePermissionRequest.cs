namespace BSourceCore.API.Contracts.Requests.Permissions;

public record UpdatePermissionRequest(
    string Name,
    string? Description);
