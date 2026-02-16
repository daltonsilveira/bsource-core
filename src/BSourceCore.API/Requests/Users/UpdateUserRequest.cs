namespace BSourceCore.API.Requests.Users;

public record UpdateUserRequest(
    string Name,
    string Email);
