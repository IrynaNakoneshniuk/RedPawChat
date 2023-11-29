namespace RedPaw.Models;

public partial class Messages
{
    public Guid Id { get; set; }

    public Guid ConversationId { get; set; }

    public Guid UserId { get; set; }

    public string Text { get; set; }

    public DateTime ? CreatedAt { get; set; }

    public DateTime ?DeletedAt { get; set; }
}
