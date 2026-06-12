using SGE.Aplicacion.Autorizacion;

namespace SGE.Infraestructura.Servicios;

public class AutorizacionProvisionalService : IAutorizacionService
{
    //CORRECCIÓN: Propiedad booleana para simular el fallo de autorización solcitado.
    public bool SimularSinPermisos { get; set; } = false;

    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso)
    {
        // Si la simulación está activada, denegamos el permiso
        if (SimularSinPermisos)
        {
            return false;
        }

        return true; // Flujo normal: permite todo para desarrollo
    }
}