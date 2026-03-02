using System.Security.Cryptography;
using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Abstractions.Services;
using BSourceCore.Application.Features.Users.Notifications.UserCreated;
using BSourceCore.Domain.Entities;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserContext _userContext;
    private readonly IPublisher _publisher;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IUserContext userContext,
        IPublisher publisher,
        ILogger<CreateUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _userContext = userContext;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<CreateUserResult> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating user with email: {Email}", request.Email);

        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser is not null)
        {
            _logger.LogWarning("User with email {Email} already exists", request.Email);
            throw new InvalidOperationException($"User with email '{request.Email}' already exists");
        }

        // Generate random 8-character password (will be reset on first access)
        var temporaryPassword = GenerateRandomPassword(8);
        var passwordHash = _passwordHasher.Hash(temporaryPassword);

        var user = new User(
            request.TenantId,
            request.Name,
            request.Email,
            passwordHash);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User created with Id: {UserId}", user.UserId);

        // Publish notification to send welcome email
        _ = Task.Run(async () => await _publisher.Publish(new UserCreatedNotification(
                user.UserId,
                user.TenantId,
                user.Name,
                user.Email,
                temporaryPassword), CancellationToken.None));

        return new CreateUserResult(user.UserId, user.Name, user.Email);
    }

    private static string GenerateRandomPassword(int length)
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789!@#$%";
        return RandomNumberGenerator.GetString(chars, length);
    }
}
