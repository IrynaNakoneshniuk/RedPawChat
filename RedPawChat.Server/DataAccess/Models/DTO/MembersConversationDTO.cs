namespace RedPawChat.Server.DataAccess.Models.DTO
{
    public class MembersConversationDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public byte[] Photo { get; set; }
        public Guid ConversationId { get; set; }
        public int IsBlocked { get; set; }
    }
}
