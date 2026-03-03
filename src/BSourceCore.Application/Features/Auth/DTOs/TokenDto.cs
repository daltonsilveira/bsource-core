namespace BSourceCore.Application.Features.Auth.DTOs;

public record TokenDto(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset ExpiresAt,
    Guid UserId,
    string Email,
    string Name,
    bool RequiresPasswordReset = false,
    string? PasswordResetToken = null);
