namespace SGE.Aplicacion.Tramites;

using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Tramites;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.ExceptionApp;
using SGE.Aplicacion.Fecha;

// Esta clase representa el caso de uso para modificar un trámite existente. 
public class ModificarTramiteUseCase(
    ITramiteRepository repo,
    IAutorizacionService auth,
    ActualizacionEstadoExpedienteService servicioEstado,
    IDateTimeProvider timeProvider)
{
    // El método Ejecutar recibe un DTO de solicitud que contiene la información necesaria para realizar la modificación, como el ID del trámite a modificar, 
    // el nuevo contenido del trámite, y el ID del usuario que realiza la modificación.
    public ModificarTramiteResponse Ejecutar(ModificarTramiteRequest request)
    {
        // 1. Validación de permisos
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.TramiteModificacion))
            throw new AutorizacionException("No tiene permisos para modificar trámites.");

        // 2. Búsqueda en repositorio
        var tramite = repo.ObtenerPorId(request.IdTramite) 
            ?? throw new RepositorioException("Trámite no encontrado.");

        // 3. Lógica de Dominio
        var fechaActual = timeProvider.ObtenerFechaActual();
        tramite.ModificarContenido(new ContenidoTramite(request.NuevoContenido), request.UsuarioId, fechaActual);

        // 4. Persistencia
        repo.Modificar(tramite);

        // 5. Actualizar el estado del expediente por si el trámite cambió
        servicioEstado.Actualizar(tramite.ExpedienteId, request.UsuarioId, fechaActual);

        // 6. Retorno de DTO
        return new ModificarTramiteResponse(true);
    }
}