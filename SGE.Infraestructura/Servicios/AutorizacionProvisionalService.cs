using SGE.Aplicacion.Autorizacion;

namespace SGE.Infraestructura.Servicios;

// Esta clase es una implementación provisional de la interfaz IAutorizacionService, que actualmente permite todos los permisos sin restricciones. Esto se hace para facilitar el desarrollo y las pruebas, pero en una implementación real, este servicio debería verificar los permisos del usuario de manera adecuada.
// La función PoseeElPermiso siempre devuelve true, lo que significa que cualquier usuario tendrá acceso a cualquier permiso. Esto es útil para evitar bloqueos en el desarrollo, pero debe ser reemplazado por una lógica de autorización real cuando corresponda.
public class AutorizacionProvisionalService : IAutorizacionService
{
    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso)
    {
        return true; // Provisional, no traba el desarrollo
    }
}