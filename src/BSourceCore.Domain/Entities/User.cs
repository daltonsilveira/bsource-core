using System.ComponentModel.DataAnnotations;
using BSourceCore.Domain.Enums;
using BSourceCore.Domain.Interfaces;

namespace BSourceCore.Domain.Entities;

public class User : TenantAuditEntity
{
    public Guid UserId { get; private set; } = Guid.NewGuid();
    [Required, MaxLength(200)]
    public string Login { get; private set; } = string.Empty;
    [Required, MaxLength(200)]
    public string Name { get; private set; } = string.Empty;
    [Required, MaxLength(200)]
    public string Email { get; private set; } = string.Empty;
    [Required, MaxLength(500)]
    public string PasswordHash { get; private set; } = string.Empty;
    [Required]
    public bool IsFirstAccess { get; private set; } = true;
    [Required]
    public BaseStatus Status { get; private set; } = BaseStatus.Active;

    // Navegação
    public ICollection<UserGroup> UserGroups { get; private set; } = new List<UserGroup>();

    private User() { }

    public User(Guid tenantId, string name, string email, string passwordHash)
    {
        UserId = Guid.NewGuid();
        TenantId = tenantId;
        Login = email;
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        IsFirstAccess = true;
        Status = BaseStatus.Pending;
        SetCreatedAudit(null);
    }

    /// <summary>
    /// Constructor for seed data - creates user without first access requirement
    /// </summary>
    public User(Guid tenantId, string name, string email, string passwordHash, bool isSeed)
    {
        UserId = Guid.NewGuid();
        TenantId = tenantId;
        Login = email;
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        IsFirstAccess = !isSeed;
        Status = isSeed ? BaseStatus.Active : BaseStatus.Pending;
        SetCreatedAudit(null);
    }

    public void Update(string name, string email, Guid? userId = null)
    {
        Name = name;
        Login = email;
        Email = email;
        SetUpdatedAudit(userId);
    }

    public void UpdatePassword(string passwordHash, Guid? userId = null)
    {
        PasswordHash = passwordHash;
        SetUpdatedAudit(userId);
    }

    public void CompleteFirstAccess(string passwordHash, Guid? userId = null)
    {
        PasswordHash = passwordHash;
        IsFirstAccess = false;
        Status = BaseStatus.Active;
        SetUpdatedAudit(userId);
    }

    public void SetStatus(BaseStatus status, Guid? userId = null)
    {
        Status = status;
        SetUpdatedAudit(userId);
    }
}
