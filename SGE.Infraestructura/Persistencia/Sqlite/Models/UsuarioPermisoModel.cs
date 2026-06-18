using System;

namespace SGE.Infraestructura.Persistencia.Sqlite.Models;

public class UsuarioPermisoModel
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string Permiso { get; set; } = string.Empty; // Guardamos el Enum como string ("ExpedienteAlta", etc.)
}