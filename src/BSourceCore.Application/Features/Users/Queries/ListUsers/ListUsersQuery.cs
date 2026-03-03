using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Users.Queries.ListUsers;

public record ListUsersQuery() : IRequest<Result<CollectionResult<UserDto>>>;
