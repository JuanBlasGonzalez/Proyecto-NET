namespace SGE.Aplicacion.Tramites;

using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Tramites;

public class ModificarTramiteUseCase(
    ITramiteRepository repo,
    IAutorizacionService auth,
    ActualizacionEstadoExpedienteService servicioEstado)
{
    // Cambiamos void por el DTO de salida
    public ModificarTramiteResponse Ejecutar(ModificarTramiteRequest request)
    {
        // 1. Validación de permisos
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.TramiteModificacion))
            throw new AutorizacionException("No tiene permisos para modificar trámites.");

        // 2. Búsqueda en repositorio (con manejo de excepción si no existe)
        var tramite = repo.ObtenerPorId(request.IdTramite) 
            ?? throw new Exception("Trámite no encontrado.");

        // 3. Lógica de Dominio
        tramite.ModificarContenido(new ContenidoTramite(request.NuevoContenido), request.UsuarioId);

        // 4. Persistencia
        repo.Modificar(tramite);

        // 5. Orquestación: Actualizar el estado del expediente por si el trámite cambió
        servicioEstado.Actualizar(tramite.ExpedienteId, request.UsuarioId);

        // 6. Retorno de DTO
        return new ModificarTramiteResponse(true);
    }
}