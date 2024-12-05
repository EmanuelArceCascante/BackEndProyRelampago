using System.ComponentModel.DataAnnotations;

namespace RelampagoP.Models
{
    public class Dependencias
    {
        [Key]
        public int IdDependencia { get; set; }
        public string? NombreDependencia { get; set; }
    }
}
