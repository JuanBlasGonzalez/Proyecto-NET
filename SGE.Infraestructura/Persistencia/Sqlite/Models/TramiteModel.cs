using System;

namespace SGE.Infraestructura.Persistencia.Sqlite.Models;

public class TramiteModel
{
    public Guid Id { get; set; } // Clave primaria del trámite
    public Guid ExpedienteId { get; set; } // Relación / Clave foránea con el Expediente
    public string Etiqueta { get; set; } = string.Empty; // El Enum guardado como string
    public string Contenido { get; set; } = string.Empty; // El Value Object extraído como string
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaUltimaModificacion { get; set; }
    public Guid UsuarioUltimoCambio { get; set; }
}