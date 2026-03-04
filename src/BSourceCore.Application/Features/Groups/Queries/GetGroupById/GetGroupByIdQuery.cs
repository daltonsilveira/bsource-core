using BSourceCore.Application.Features.Groups.DTOs;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Groups.Queries.GetGroupById;

public record GetGroupByIdQuery(Guid GroupId) : IRequest<Result<GroupDto>>;
