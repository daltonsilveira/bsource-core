using System.Security.Cryptography;
using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Abstractions.Services;
using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Application.Features.Users.Notifications.UserCreated;
using BSourceCore.Application.Features.Users.Queries.GetUserById;
using BSourceCore.Domain.Entities;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserContext _userContext;
    private readonly IPublisher _publisher;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IUserContext userContext,
        IPublisher publisher,
        IMediator mediator,
        ILogger<CreateUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _userContext = userContext;
        _publisher = publisher;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Result<UserDto>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating user with email: {Email}", request.Email);

        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser is not null)
        {
            _logger.LogWarning("User with email {Email} already exists", request.Email);
            return Result<UserDto>.Fail(new Error(
                "User.EmailConflict",
                $"User with email '{request.Email}' already exists",
                ErrorType.Conflict));
        }

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

        _ = Task.Run(async () => await _publisher.Publish(new UserCreatedNotification(
                user.UserId,
                user.TenantId,
                user.Name,
                user.Email,
                temporaryPassword), CancellationToken.None));

        var result = await _mediator.Send(new GetUserByIdQuery(user.UserId), cancellationToken);

        return Result<UserDto>.Success(result.Value!);
    }

    private static string GenerateRandomPassword(int length)
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789!@#$%";
        return RandomNumberGenerator.GetString(chars, length);
    }
}
