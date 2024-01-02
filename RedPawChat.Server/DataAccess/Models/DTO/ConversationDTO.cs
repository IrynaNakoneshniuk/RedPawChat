using RedPaw.Models;

namespace RedPawChat.Server.DataAccess.Models.DTO
{
    public class ConversationDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<MessagesDTO> Messages { get; set; }=new List<MessagesDTO>();
    }
}
