namespace BSourceCore.Shared.Abstractions;

public interface IUserContext
{
    Guid? TenantId { get; }
    Guid? UserId { get; }
    string Email { get; }
    string IpAddress { get; }
    string UserAgent { get; }
    bool IsAuthenticated { get; }
}
