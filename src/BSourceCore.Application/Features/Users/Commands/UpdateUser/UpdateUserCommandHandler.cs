using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Application.Features.Users.Queries.GetUserById;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IMediator mediator,
        ILogger<UpdateUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Result<UserDto>> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating user: {UserId}", request.UserId);

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User not found with Id: {UserId}", request.UserId);
            return Result<UserDto>.Fail(new Error(
                "User.NotFound",
                $"User with Id '{request.UserId}' not found",
                ErrorType.NotFound));
        }

        user.Update(request.Name, request.Email);

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User updated: {UserId}", user.UserId);

        var result = await _mediator.Send(new GetUserByIdQuery(user.UserId), cancellationToken);

        return Result<UserDto>.Success(result.Value!);
    }
}
