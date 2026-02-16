using BSourceCore.Application.Features.Users.DTOs;
using MediatR;

namespace BSourceCore.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery(Guid TenantId) : IRequest<IEnumerable<UserDto>>;
