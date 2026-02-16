namespace BSourceCore.Shared.Abstractions;

public interface ITenantContext
{
    Guid? TenantId { get; }
}
