using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.PasswordResets.Commands.ConfirmPasswordReset;

public class ConfirmPasswordResetCommandHandler : IRequestHandler<ConfirmPasswordResetCommand, ConfirmPasswordResetResult>
{
    private readonly IPasswordResetRepository _passwordResetRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ConfirmPasswordResetCommandHandler> _logger;

    public ConfirmPasswordResetCommandHandler(
        IPasswordResetRepository passwordResetRepository,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        ILogger<ConfirmPasswordResetCommandHandler> logger)
    {
        _passwordResetRepository = passwordResetRepository;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ConfirmPasswordResetResult> Handle(
        ConfirmPasswordResetCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing password reset confirmation");

        var passwordReset = await _passwordResetRepository.GetByTokenAsync(request.Token, cancellationToken);

        if (passwordReset is null)
        {
            _logger.LogWarning("Password reset token not found");
            throw new InvalidOperationException("Invalid or expired token");
        }

        if (!passwordReset.IsValid())
        {
            _logger.LogWarning("Password reset token is invalid or expired for user {UserId}", passwordReset.UserId);
            throw new InvalidOperationException("Invalid or expired token");
        }

        var user = await _userRepository.GetByIdAsync(passwordReset.UserId, cancellationToken);
        if (user is null)
        {
            _logger.LogWarning("User not found for password reset {PasswordResetId}", passwordReset.PasswordResetId);
            throw new InvalidOperationException("User not found");
        }

        // Hash the new password and complete first access
        var passwordHash = _passwordHasher.Hash(request.Password);
        user.CompleteFirstAccess(passwordHash);
        _userRepository.Update(user);

        // Invalidate the password reset token
        passwordReset.Invalidate();
        _passwordResetRepository.Update(passwordReset);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Password reset completed successfully for user {UserId}",
            user.UserId);

        return new ConfirmPasswordResetResult(true);
    }
}
