namespace BSourceCore.API.Contracts.Requests.Auth;

public record LoginRequest(
    string Email,
    string Password,
    Guid? TenantId = null);
