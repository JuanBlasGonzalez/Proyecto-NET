using System;

namespace SGE.Aplicacion.Autorizacion;

// Esta interfaz define el contrato para un servicio de autorización en la aplicación.
// El método PoseeElPermiso toma un identificador de usuario y un permiso, y devuelve un booleano indicando si el usuario tiene ese permiso o no.
public interface IAutorizacionService
{
    bool PoseeElPermiso(Guid idUsuario, Permiso permiso);
}
