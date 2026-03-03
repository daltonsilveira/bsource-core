using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Abstractions.Services;
using BSourceCore.Application.Features.Auth.DTOs;
using BSourceCore.Application.Models.Requests;
using BSourceCore.Domain.Entities;
using BSourceCore.Domain.Enums;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenDto>>
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

    public async Task<Result<TokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByLoginAsync(request.Login, cancellationToken);

        if (user is null)
        {
            return Result<TokenDto>.Fail(new Error(
                "Auth.InvalidCredentials", "Invalid login or password", ErrorType.Unauthorized));
        }

        if (user.Status != BaseStatus.Active && user.Status != BaseStatus.Pending)
        {
            return Result<TokenDto>.Fail(new Error(
                "Auth.InactiveAccount", "User account is not active", ErrorType.Unauthorized));
        }

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Result<TokenDto>.Fail(new Error(
                "Auth.InvalidCredentials", "Invalid login or password", ErrorType.Unauthorized));
        }

        if (user.IsFirstAccess)
        {
            return await HandleFirstAccessAsync(user, cancellationToken);
        }

        var tokenResult = await _tokenService.GenerateTokenAsync(new TokenSubject(
            user.UserId,
            user.Name,
            user.Email,
            user.TenantId
        ), cancellationToken);

        return Result<TokenDto>.Success(new TokenDto(
            tokenResult.AccessToken,
            tokenResult.RefreshToken,
            tokenResult.ExpiresAt,
            user.UserId,
            user.Email,
            user.Name));
    }

    private async Task<Result<TokenDto>> HandleFirstAccessAsync(User user, CancellationToken cancellationToken)
    {
        var activeResets = await _passwordResetRepository.GetActiveByUserIdAsync(user.UserId, cancellationToken);
        foreach (var reset in activeResets)
        {
            reset.Invalidate();
            _passwordResetRepository.Update(reset);
        }

        var passwordReset = new PasswordReset(user.UserId);
        await _passwordResetRepository.AddAsync(passwordReset, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<TokenDto>.Success(new TokenDto(
            AccessToken: string.Empty,
            RefreshToken: string.Empty,
            ExpiresAt: passwordReset.ExpiresAt,
            UserId: user.UserId,
            Email: user.Email,
            Name: user.Name,
            RequiresPasswordReset: true,
            PasswordResetToken: passwordReset.Token));
    }
}
