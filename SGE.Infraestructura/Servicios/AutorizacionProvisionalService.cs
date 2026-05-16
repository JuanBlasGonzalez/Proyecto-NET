using SGE.Aplicacion.Autorizacion;

namespace SGE.Infraestructura.Servicios;

public class AutorizacionProvisionalService : IAutorizacionService
{
    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso)
    {
        return true; // Provisional, no traba el desarrollo
    }
}