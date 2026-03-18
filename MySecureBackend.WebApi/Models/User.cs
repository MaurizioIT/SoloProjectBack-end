using System.ComponentModel.DataAnnotations.Schema;

namespace MySecureBackend.WebApi.Models
{
    [Table("User")]
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
