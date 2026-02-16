namespace BSourceCore.Application.Features.Tenants.DTOs;

public record TenantDto(
    Guid TenantId,
    string Name,
    string Slug,
    string? Description,
    string Status,
    DateTimeOffset CreatedAt);
