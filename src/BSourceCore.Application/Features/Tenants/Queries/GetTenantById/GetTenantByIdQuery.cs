using BSourceCore.Application.Features.Tenants.DTOs;
using MediatR;

namespace BSourceCore.Application.Features.Tenants.Queries.GetTenantById;

public record GetTenantByIdQuery(Guid TenantId) : IRequest<TenantDto?>;
