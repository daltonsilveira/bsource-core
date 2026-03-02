namespace BSourceCore.API.Contracts.Requests.Users;

public record UpdateUserRequest(
    string Name,
    string Email);
