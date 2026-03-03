namespace BSourceCore.API.Contracts.Responses;

public record TokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset ExpiresAt,
    Guid UserId,
    string Email,
    string Name,
    bool RequiresPasswordReset = false,
    string? PasswordResetToken = null);
