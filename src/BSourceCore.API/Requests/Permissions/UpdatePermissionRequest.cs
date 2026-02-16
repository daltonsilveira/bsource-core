namespace BSourceCore.API.Requests.Permissions;

public record UpdatePermissionRequest(
    string Name,
    string? Description);
