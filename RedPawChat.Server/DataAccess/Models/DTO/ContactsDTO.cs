namespace RedPawChat.Server.DataAccess.Models.DTO
{
    public class ContactsDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] Photo { get; set; }
        public string Nick { get; set; }
        public int IsOnline { get; set; }
        public int IsBlock {  get; set; }
    }
}