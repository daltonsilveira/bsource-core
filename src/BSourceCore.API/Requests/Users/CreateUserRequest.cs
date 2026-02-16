namespace BSourceCore.API.Requests.Users;

public record CreateUserRequest(
    Guid TenantId,
    string Name,
    string Email);
