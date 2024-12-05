using System.ComponentModel.DataAnnotations;

namespace RelampagoP.Models
{
    public class Area
    {
        [Key]
        public int IdArea { get; set; }
        public string? NombreArea { get; set; }
    }
}
