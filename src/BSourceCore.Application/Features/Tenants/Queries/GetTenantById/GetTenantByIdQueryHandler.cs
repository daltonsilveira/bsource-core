using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Tenants.DTOs;
using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Tenants.Queries.GetTenantById;

public class GetTenantByIdQueryHandler : IRequestHandler<GetTenantByIdQuery, Result<TenantDto>>
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

    public async Task<Result<TenantDto>> Handle(
        GetTenantByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting tenant by Id: {TenantId}", request.TenantId);

        var tenant = await _tenantRepository.GetByIdAsync(request.TenantId, cancellationToken);

        if (tenant is null)
        {
            _logger.LogWarning("Tenant not found with Id: {TenantId}", request.TenantId);
            return Result<TenantDto>.Fail(new Error(
                "Tenant.NotFound",
                $"Tenant with Id '{request.TenantId}' not found",
                ErrorType.NotFound));
        }

        return Result<TenantDto>.Success(new TenantDto(
            tenant.TenantId,
            tenant.Name,
            tenant.Slug,
            tenant.Description,
            tenant.Status,
            tenant.CreatedAt,
            tenant.CreatedBy != null ? new UserAuditDto(tenant.CreatedBy.UserId, tenant.CreatedBy.Name) : null,
            tenant.UpdatedAt,
            tenant.UpdatedBy != null ? new UserAuditDto(tenant.UpdatedBy.UserId, tenant.UpdatedBy.Name) : null
            ));
    }
}
