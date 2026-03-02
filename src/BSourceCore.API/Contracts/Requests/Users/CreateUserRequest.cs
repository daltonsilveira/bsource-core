namespace BSourceCore.API.Contracts.Requests.Users;

public record CreateUserRequest(
    Guid TenantId,
    string Name,
    string Email);
