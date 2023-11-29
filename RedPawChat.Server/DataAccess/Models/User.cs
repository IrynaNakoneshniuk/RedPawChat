namespace RedPaw.Models;

public  class User
{
    public Guid Id { get; set; }
    public string NickName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public int IsActive { get; set; }
    public int IsBlocked { get; set; }
    public string Role { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public string Theme { get; set; } = null!;
    public byte[]? ImageData { get; set; }
    public virtual IEnumerable<BlackList> ? BlackListUsers { get; set; } 
    public virtual IEnumerable<Contacts> ? ContactUsers { get; set; }
    public virtual IEnumerable<GroupMember> ? GroupMembers { get; set; }
    public virtual IEnumerable<Conversations> ? Conversations  { get; set; } 
}
