using System;

namespace SGE.Aplicacion.Autorizacion;

public interface IAutorizacionService
{
    bool PoseeElPermiso(Guid idUsuario, Permiso permiso);
}
