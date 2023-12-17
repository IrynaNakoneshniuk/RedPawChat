using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RedPawChat.Server.DataAccess.Models
{
    public class LoginInfo
    {

        public LoginInfo() // Доданий конструктор без параметрів
        {
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
