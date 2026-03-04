using BSourceCore.Application.Features.Tenants.DTOs;
using MediatR;

namespace BSourceCore.Application.Features.Tenants.Queries.GetTenants;

public record ListTenantsQuery : IRequest<IEnumerable<TenantDto>>;
