using System.ComponentModel.DataAnnotations;

namespace RelampagoP.Models
{
    public class Procedimiento
    {
        [Key]
        public int IdProcedimiento { get; set; }
        public int? IdEje { get; set; }
        public int? IdArea { get; set; }
        public int? IdDependencia { get; set; }
        public string? TipoProcedimiento { get; set; }
        public string? Estado { get; set; }
        public string? Teletrabajado { get; set; }
        public int? IdMacroproceso { get; set; }
        public int? IdEjeEstrategico { get; set; }
        public string? TipoDocumento { get; set; }
        public string? NombreProcedimiento { get; set; }
        public string? ApoyoTecnologico { get; set; }
        public int? AnioActualizacion { get; set; }
    }
}
