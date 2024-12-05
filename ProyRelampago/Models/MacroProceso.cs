using System.ComponentModel.DataAnnotations;

namespace RelampagoP.Models
{
    public class MacroProceso
    {
        [Key]
        public int IdMacroproceso { get; set; }
        public string? NombreMacroproceso { get; set; }
    }
}
