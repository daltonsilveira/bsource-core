using BSourceCore.Application.Features.Groups.DTOs;
using MediatR;

namespace BSourceCore.Application.Features.Groups.Queries.GetGroupById;

public record GetGroupByIdQuery(Guid GroupId) : IRequest<GroupDto?>;
