using RedPaw.Models;

namespace RedPawChat.Server.DataAccess.Models.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Name {  get; set; }
        public string Email { get; set; }
        public byte[] Photo { get; set; }
        public string Nick {  get; set; }
        public int IsOnline { get; set; }
        public List<ConversationDTO> Conversation { get; set; } = new List<ConversationDTO>();
        public List<ContactsDTO> Contacts { get; set; } = new List<ContactsDTO>();
        //public IEnumerable<BlackListDTO> BlackLists { get; set; } = new List<BlackListDTO>();

    }
}
