using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySecureBackend.WebApi.Models
{
    [Table("Environment2D")]
    public class Environment2D
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        public int MaxHeight { get; set; }
        public int MaxLength { get; set; }
    }
}

