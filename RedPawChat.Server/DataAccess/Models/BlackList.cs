namespace RedPaw.Models;

public partial class BlackList
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid ParticipantId { get; set; }

    public DateTime? CreatedAt { get; set; }

}
