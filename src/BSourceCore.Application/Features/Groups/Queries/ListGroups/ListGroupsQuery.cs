using BSourceCore.Application.Features.Groups.DTOs;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Groups.Queries.ListGroups;

public record ListGroupsQuery(Guid TenantId) : IRequest<Result<CollectionResult<GroupDto>>>;
