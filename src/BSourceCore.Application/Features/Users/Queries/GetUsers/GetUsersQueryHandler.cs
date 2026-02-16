using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly ILogger<GetUsersQueryHandler> _logger;

    public GetUsersQueryHandler(
        IUserRepository userRepository,
        IUserContext userContext,
        ILogger<GetUsersQueryHandler> logger)
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<IEnumerable<UserDto>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting users for tenant: {TenantId}", request.TenantId);

        var users = await _userRepository.GetAllByTenantAsync(request.TenantId, cancellationToken);

        return users.Select(u => new UserDto(
            u.UserId,
            u.TenantId,
            u.Name,
            u.Email,
            u.Status.ToString(),
            u.CreatedAt));
    }
}
