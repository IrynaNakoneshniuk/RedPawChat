using Microsoft.AspNetCore.Identity;

namespace RedPaw.Models;

public  class User: IdentityUser
{
    public Guid Id { get; set; }
    public string NickName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public int IsActive { get; set; }
    public int IsBlocked { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string Theme { get; set; } = null!;
    public byte[]? ImageData { get; set; }
    public string ?UserName { get; set; }
    public string? NormalizedUserName { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }
    public virtual IEnumerable<BlackList> ? BlackListUsers { get; set; } 
    public virtual IEnumerable<Contacts> ? ContactUsers { get; set; }
    public virtual IEnumerable<GroupMember> ? GroupMembers { get; set; }
    public virtual IEnumerable<Conversations> ? Conversations  { get; set; } 
}
