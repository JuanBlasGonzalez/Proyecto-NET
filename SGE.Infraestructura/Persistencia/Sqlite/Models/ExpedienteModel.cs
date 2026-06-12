using System;

namespace SGE.Infraestructura.Persistencia.Sqlite.Models;

public class ExpedienteModel
{
    public Guid Id { get; set; }
    public string Caratula { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaUltimaModificacion { get; set; } 
    public Guid UsuarioUltimoCambio { get; set; }
}