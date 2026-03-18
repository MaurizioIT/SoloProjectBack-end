using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySecureBackend.WebApi.Models
{
    [Table("Object2D")]
    public class Object2D
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int EnvironmentID { get; set; }

        [Required]
        public int PrefabID { get; set; }

        [Required]
        public float PositionX { get; set; }

        [Required]
        public float PositionY { get; set; }

        [Required]
        public float ScaleX { get; set; }

        [Required]
        public float ScaleY { get; set; }

        [Required]
        public float RotationZ { get; set; }

        [Required]
        public int SortingLayer { get; set; }
    }
}