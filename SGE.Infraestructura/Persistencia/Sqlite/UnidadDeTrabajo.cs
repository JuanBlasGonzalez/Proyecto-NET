using SGE.Aplicacion.Interfaces;

namespace SGE.Infraestructura.Persistencia.Sqlite;

public class UnidadDeTrabajo : IUnidadDeTrabajo
{
    private readonly SgeContext _context;

    public UnidadDeTrabajo(SgeContext context)
    {
        _context = context;
    }

    public void Guardar()
    {
        // Aplica la transaccionalidad real en SQLite
        _context.SaveChanges();
    }
}