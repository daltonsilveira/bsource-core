using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
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

    public async Task<UserDto?> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting user by Id: {UserId}", request.UserId);

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User not found with Id: {UserId}", request.UserId);
            return null;
        }

        return new UserDto(
            user.UserId,
            user.TenantId,
            user.Name,
            user.Email,
            user.Status.ToString(),
            user.CreatedAt);
    }
}
