using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(
        IUserRepository userRepository,
        IUserContext userContext,
        ILogger<GetUserByIdQueryHandler> logger)
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result<UserDto>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting user by Id: {UserId}", request.UserId);

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User not found with Id: {UserId}", request.UserId);
            return Result<UserDto>.Fail(new Error(
                "User.NotFound",
                $"User with Id '{request.UserId}' not found",
                ErrorType.NotFound));
        }

        return Result<UserDto>.Success(new UserDto(
            user.UserId,
            user.TenantId,
            user.Name,
            user.Email,
            user.Status,
            user.CreatedAt,
            user.CreatedBy != null ? new UserAuditDto(
                user.CreatedBy.UserId,
                user.CreatedBy.Name) : null,
            user.UpdatedAt,
            user.UpdatedBy != null ? new UserAuditDto(
                user.UpdatedBy.UserId,
                user.UpdatedBy.Name) : null));
    }
}
