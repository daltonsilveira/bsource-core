using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Features.Tenants.DTOs;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Tenants.Queries.GetTenantById;

public class GetTenantByIdQueryHandler : IRequestHandler<GetTenantByIdQuery, TenantDto?>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserContext _userContext;
    private readonly ILogger<GetTenantByIdQueryHandler> _logger;

    public GetTenantByIdQueryHandler(
        ITenantRepository tenantRepository,
        IUserContext userContext,
        ILogger<GetTenantByIdQueryHandler> logger)
    {
        _tenantRepository = tenantRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<TenantDto?> Handle(
        GetTenantByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting tenant by Id: {TenantId}", request.TenantId);

        var tenant = await _tenantRepository.GetByIdAsync(request.TenantId, cancellationToken);

        if (tenant is null)
        {
            _logger.LogWarning("Tenant not found with Id: {TenantId}", request.TenantId);
            return null;
        }

        return new TenantDto(
            tenant.TenantId,
            tenant.Name,
            tenant.Slug,
            tenant.Description,
            tenant.Status.ToString(),
            tenant.CreatedAt);
    }
}
