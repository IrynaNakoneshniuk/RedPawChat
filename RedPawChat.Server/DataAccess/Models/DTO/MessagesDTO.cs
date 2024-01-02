namespace RedPawChat.Server.DataAccess.Models.DTO
{
    public class MessagesDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public byte[] Photo { get; set; }
        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}