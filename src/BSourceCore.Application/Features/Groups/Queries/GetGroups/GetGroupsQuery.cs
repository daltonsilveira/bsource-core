using BSourceCore.Application.Features.Groups.DTOs;
using MediatR;

namespace BSourceCore.Application.Features.Groups.Queries.GetGroups;

public record GetGroupsQuery(Guid TenantId) : IRequest<IEnumerable<GroupDto>>;
