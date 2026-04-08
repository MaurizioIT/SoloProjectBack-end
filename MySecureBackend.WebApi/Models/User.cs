using System.ComponentModel.DataAnnotations.Schema;

namespace MySecureBackend.WebApi.Models
{
    [Table("User")]
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
