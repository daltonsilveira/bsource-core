namespace BSourceCore.API.Contracts.Requests.Permissions;

public record CreatePermissionRequest(
    string Code,
    string Name,
    string? Description);
