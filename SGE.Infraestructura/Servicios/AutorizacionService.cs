using System;
using System.Linq;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Interfaces;
using SGE.Dominio.Usuarios;

namespace SGE.Infraestructura.Servicios;

public class AutorizacionService(IUsuarioRepository repoUsuario) : IAutorizacionService
{
    public bool PoseeElPermiso(Guid usuarioId, Permiso permisoRequerido)
    {
        var usuario = repoUsuario.ObtenerPorId(usuarioId);
        
        // Si el usuario no existe, denegamos de inmediato
        if (usuario == null) return false;

        // Si el usuario tiene el flag EsAdministrador == true, tiene acceso total implícito
        if (usuario.EsAdministrador) return true;

        // REGLA DE IMPLICANCIA (Punto 3.3): Si se pide modificar o borrar, y tiene el permiso superior de "Baja", se le concede
        if (permisoRequerido == Permiso.ExpedienteModificacion && usuario.Permisos.Contains(Permiso.ExpedienteBaja))
            return true;

        if (permisoRequerido == Permiso.TramiteModificacion && usuario.Permisos.Contains(Permiso.TramiteBaja))
            return true;

        // Validación efectiva estándar de la colección interna de permisos
        return usuario.Permisos.Contains(permisoRequerido);
    }
}