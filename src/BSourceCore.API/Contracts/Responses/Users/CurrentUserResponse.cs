using BSourceCore.Application.Features.Users.Queries.GetCurrentUser;

namespace BSourceCore.API.Contracts.Responses;

public class CurrentUserResponse
{
    public Guid UserId { get; }
    public string Email { get; }
    public string Name { get; }
    public Guid TenantId { get; }
    public IEnumerable<string> PermissionCodes { get; }
    public IEnumerable<NotificationResponse> Notifications { get; }

    public CurrentUserResponse(CurrentUserDto dto)
    {
        UserId = dto.UserId;
        Email = dto.Email;
        Name = dto.Name;
        TenantId = dto.TenantId;
        PermissionCodes = dto.PermissionCodes;
        Notifications = dto.Notifications.Select(n => new NotificationResponse(n));
    }
}
