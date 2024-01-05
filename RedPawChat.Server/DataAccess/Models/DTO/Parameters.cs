using System.ComponentModel.DataAnnotations;

namespace RedPawChat.Server.DataAccess.Models.DTO
{
    public class Parameters
    {
        public Guid СontactId { get; set; }
        public Guid UserId { get; set; }
       
        public Parameters()
        {

        }
    }
}
