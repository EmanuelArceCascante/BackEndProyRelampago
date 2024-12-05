using System.ComponentModel.DataAnnotations;

namespace RelampagoP.Models
{
    public class Eje
    {
        [Key]
        public int IdEje { get; set; }
        public string? NombreEjeEstrategico { get; set; }
    }
}
