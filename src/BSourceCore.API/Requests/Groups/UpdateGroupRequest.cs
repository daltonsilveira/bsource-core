namespace BSourceCore.API.Requests.Groups;

public record UpdateGroupRequest(
    string Name,
    string? Description);
