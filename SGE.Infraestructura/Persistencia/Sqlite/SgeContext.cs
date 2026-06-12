using Microsoft.EntityFrameworkCore;
using SGE.Infraestructura.Persistencia.Sqlite.Models;

namespace SGE.Infraestructura.Persistencia.Sqlite;

public class SgeContext : DbContext
{
    public DbSet<ExpedienteModel> Expedientes => Set<ExpedienteModel>();
    public DbSet<TramiteModel> Tramites => Set<TramiteModel>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configuramos la base de datos SQLite persistida en un archivo local
        optionsBuilder.UseSqlite("Data Source=sge.db");
    }
}