using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Abstractions.Services;
using BSourceCore.Application.Features.Auth.DTOs;
using BSourceCore.Domain.Entities;
using BSourceCore.Domain.Enums;
using MediatR;

namespace BSourceCore.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetRepository _passwordResetRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordResetRepository passwordResetRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordResetRepository = passwordResetRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<TokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByLoginAsync(request.Login, cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid login or password");
        }

        // Allow login for Active or Pending (first access) users
        if (user.Status != BaseStatus.Active && user.Status != BaseStatus.Pending)
        {
            throw new UnauthorizedAccessException("User account is not active");
        }

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid login or password");
        }

        // Check if user needs to reset password on first access
        if (user.IsFirstAccess)
        {
            return await HandleFirstAccessAsync(user, cancellationToken);
        }

        // Get user permissions through groups
        var permissions = await _userRepository.GetUserPermissionsAsync(user.UserId, cancellationToken);

        // Generate tokens
        var tokenResult = await _tokenService.GenerateTokenAsync(user, permissions, cancellationToken);

        return new TokenDto(
            tokenResult.AccessToken,
            tokenResult.RefreshToken,
            tokenResult.ExpiresAt,
            user.UserId,
            user.Email,
            user.Name,
            permissions.Select(p => p.Code));
    }

    private async Task<TokenDto> HandleFirstAccessAsync(User user, CancellationToken cancellationToken)
    {
        // Invalidate any existing active password resets for this user
        var activeResets = await _passwordResetRepository.GetActiveByUserIdAsync(user.UserId, cancellationToken);
        foreach (var reset in activeResets)
        {
            reset.Invalidate();
            _passwordResetRepository.Update(reset);
        }

        // Create new password reset
        var passwordReset = new PasswordReset(user.UserId);
        await _passwordResetRepository.AddAsync(passwordReset, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TokenDto(
            AccessToken: string.Empty,
            RefreshToken: string.Empty,
            ExpiresAt: passwordReset.ExpiresAt,
            UserId: user.UserId,
            Email: user.Email,
            Name: user.Name,
            Permissions: Enumerable.Empty<string>(),
            RequiresPasswordReset: true,
            PasswordResetToken: passwordReset.Token);
    }
}
