namespace BSourceCore.API.Requests.Auth;

public record LoginRequest(
    string Email,
    string Password,
    Guid? TenantId = null);
