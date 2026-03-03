using BSourceCore.Application.Features.Groups.DTOs;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Groups.Queries.GetGroups;

public record GetGroupsQuery(Guid TenantId) : IRequest<Result<CollectionResult<GroupDto>>>;
