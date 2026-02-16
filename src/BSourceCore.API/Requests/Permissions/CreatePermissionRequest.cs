namespace BSourceCore.API.Requests.Permissions;

public record CreatePermissionRequest(
    string Code,
    string Name,
    string? Description);
