namespace BSourceCore.API.Responses;

public record TokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset ExpiresAt,
    Guid UserId,
    string Email,
    string Name,
    IEnumerable<string> Permissions,
    bool RequiresPasswordReset = false,
    string? PasswordResetToken = null);
