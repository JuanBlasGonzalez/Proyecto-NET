using System;
using System.Collections.Generic;

namespace SGE.Infraestructura.Persistencia.Sqlite.Models;

public class UsuarioModel
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string ContrasenaHash { get; set; } = string.Empty;
    public bool EsAdministrador { get; set; }

    // Relación uno a muchos con la tabla de permisos
    public List<UsuarioPermisoModel> Permisos { get; set; } = new();
}