using Asp.Versioning;
using BSourceCore.API.Contracts.Requests.Tenants;
using BSourceCore.API.Contracts.Responses;
using BSourceCore.Application.Features.Tenants.Commands.CreateTenant;
using BSourceCore.Application.Features.Tenants.Queries.GetTenantById;
using BSourceCore.Application.Features.Tenants.Queries.GetTenants;
using BSourceCore.Shared.Kernel.Results;
using BSourceCore.API.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BSourceCore.Application.Features.Tenants.DTOs;

namespace BSourceCore.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Authorize]
public class TenantsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TenantsController> _logger;

    public TenantsController(IMediator mediator, ILogger<TenantsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new tenant
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CollectionResponse<TenantResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "tenants.create")]
    public async Task<IActionResult> Create([FromBody] CreateTenantRequest request)
    {
        _logger.LogInformation("Creating tenant with name: {Name}", request.Name);

        var result = await _mediator.Send(new CreateTenantCommand(
            request.Name,
            request.Slug,
            request.Description));

        if (!result.IsSuccess) return result.ToProblemDetails(this);

        return Ok(CollectionResult<TenantResponse>.From(ToDefaultResponse(result.Value!)));
    }

    /// <summary>
    /// Gets a tenant by ID
    /// </summary>
    [HttpGet("{tenantId:guid}")]
    [ProducesResponseType(typeof(CollectionResponse<TenantResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "tenants.read")]
    public async Task<IActionResult> GetById(Guid tenantId)
    {
        _logger.LogInformation("Getting tenant by Id: {TenantId}", tenantId);

        var result = await _mediator.Send(new GetTenantByIdQuery(tenantId));

        if (!result.IsSuccess) return result.ToProblemDetails(this);

        return Ok(CollectionResponse<TenantResponse>.From(ToDefaultResponse(result.Value!)));
    }

    /// <summary>
    /// Gets all tenants
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(CollectionResponse<TenantResponse>), StatusCodes.Status200OK)]
    [Authorize(Policy = "tenants.read")]
    public async Task<IActionResult> List()
    {
        _logger.LogInformation("Listing all tenants");

        var result = await _mediator.Send(new ListTenantsQuery());

        var response = result.Select(t => ToDefaultResponse(t));

        return Ok(CollectionResponse<TenantResponse>.From(response));
    }

    private TenantResponse ToDefaultResponse(TenantDto dto)
    {
        return new TenantResponse(
            dto.TenantId, 
            dto.Name,
            dto.Slug,
            dto.Description,
            dto.Status.ToString(), 
            dto.CreatedAt,
            dto.CreatedBy != null ? new UserAuditResponse(
                dto.CreatedBy.UserId,
                dto.CreatedBy.Name) : null,
            dto.UpdatedAt,
            dto.UpdatedBy != null ? new UserAuditResponse(
                dto.UpdatedBy.UserId,
                dto.UpdatedBy.Name) : null
            );
    }
}
