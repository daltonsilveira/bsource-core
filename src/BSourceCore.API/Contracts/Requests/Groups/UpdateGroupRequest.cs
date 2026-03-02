namespace BSourceCore.API.Contracts.Requests.Groups;

public record UpdateGroupRequest(
    string Name,
    string? Description);
