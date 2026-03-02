using Asp.Versioning;
using BSourceCore.API.Contracts.Requests.Tenants;
using BSourceCore.API.Contracts.Responses;
using BSourceCore.Application.Features.Tenants.Commands.CreateTenant;
using BSourceCore.Application.Features.Tenants.Queries.GetTenantById;
using BSourceCore.Application.Features.Tenants.Queries.GetTenants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    [ProducesResponseType(typeof(ApiResponse<TenantResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "tenants.create")]
    public async Task<IActionResult> Create([FromBody] CreateTenantRequest request)
    {
        _logger.LogInformation("Creating tenant with name: {Name}", request.Name);

        var command = new CreateTenantCommand(
            request.Name,
            request.Slug,
            request.Description);

        var result = await _mediator.Send(command);

        var response = new TenantResponse(
            result.TenantId,
            result.Name,
            result.Slug,
            request.Description,
            "Active",
            DateTimeOffset.UtcNow);

        return CreatedAtAction(
            nameof(GetById),
            new { tenantId = result.TenantId },
            ApiResponse<TenantResponse>.Ok(response));
    }

    /// <summary>
    /// Gets a tenant by ID
    /// </summary>
    [HttpGet("{tenantId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<TenantResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "tenants.read")]
    public async Task<IActionResult> GetById(Guid tenantId)
    {
        _logger.LogInformation("Getting tenant by Id: {TenantId}", tenantId);

        var query = new GetTenantByIdQuery(tenantId);
        var result = await _mediator.Send(query);

        if (result is null)
        {
            return NotFound(ApiResponse.Fail($"Tenant with Id '{tenantId}' not found"));
        }

        var response = new TenantResponse(
            result.TenantId,
            result.Name,
            result.Slug,
            result.Description,
            result.Status,
            result.CreatedAt);

        return Ok(ApiResponse<TenantResponse>.Ok(response));
    }

    /// <summary>
    /// Gets all tenants
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TenantResponse>>), StatusCodes.Status200OK)]
    [Authorize(Policy = "tenants.read")]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Getting all tenants");

        var query = new GetTenantsQuery();
        var result = await _mediator.Send(query);

        var response = result.Select(t => new TenantResponse(
            t.TenantId,
            t.Name,
            t.Slug,
            t.Description,
            t.Status,
            t.CreatedAt));

        return Ok(ApiResponse<IEnumerable<TenantResponse>>.Ok(response));
    }
}
