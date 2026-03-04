using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Users.Queries.ListUsers;

public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, Result<CollectionResult<UserDto>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly ILogger<ListUsersQueryHandler> _logger;

    public ListUsersQueryHandler(
        IUserRepository userRepository,
        IUserContext userContext,
        ILogger<ListUsersQueryHandler> logger)
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result<CollectionResult<UserDto>>> Handle(
        ListUsersQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting users for tenant: {TenantId}", _userContext.TenantId);

        var users = await _userRepository.GetAllAsync(cancellationToken);

        var items = users.Select(u => new UserDto(
            u.UserId,
            u.TenantId,
            u.Name,
            u.Email,
            u.Status,
            u.CreatedAt,
            u.CreatedBy != null ? new UserAuditDto(
                u.CreatedBy.UserId,
                u.CreatedBy.Name) : null,
            u.UpdatedAt,
            u.UpdatedBy != null ? new UserAuditDto(
                u.UpdatedBy.UserId,
                u.UpdatedBy.Name) : null)).ToList();

        return Result<CollectionResult<UserDto>>.Success(CollectionResult<UserDto>.From(items));
    }
}
