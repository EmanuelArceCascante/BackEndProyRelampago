using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using RelampagoP.Models;

namespace ProyRelampago.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Area> Area { get; set; }
        public DbSet<Eje> Eje { get; set; }
        public DbSet<Dependencias> Dependencia { get; set; }
        public DbSet<Procedimiento> Procedimiento { get; set; }
        public DbSet<MacroProceso> Macroproceso { get; set; }

    }
}
