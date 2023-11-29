namespace RedPaw.Models;

public partial class Conversations
{
    public Guid Id { get; set; }
    public DateTime ?CreatedAt { get; set; }
    public DateTime ?UpdatedAt { get; set; }
    public string Title { get; set; }
    public virtual IEnumerable<Messages> ? Messages { get; set; } 
}
