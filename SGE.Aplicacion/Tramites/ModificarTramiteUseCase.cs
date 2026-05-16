namespace SGE.Aplicacion.Tramites;

using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Tramites;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.ExceptionApp;
using SGE.Aplicacion.Fecha;

public class ModificarTramiteUseCase(
    ITramiteRepository repo,
    IAutorizacionService auth,
    ActualizacionEstadoExpedienteService servicioEstado,
    IDateTimeProvider timeProvider)
{
    // Cambiamos void por el DTO de salida
    public ModificarTramiteResponse Ejecutar(ModificarTramiteRequest request)
    {
        // 1. Validación de permisos
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.TramiteModificacion))
            throw new AutorizacionException("No tiene permisos para modificar trámites.");

        // 2. Búsqueda en repositorio (con manejo de excepción si no existe)
        var tramite = repo.ObtenerPorId(request.IdTramite) 
            ?? throw new RepositorioException("Trámite no encontrado.");

        // 3. Lógica de Dominio
        var fechaActual = timeProvider.ObtenerFechaActual();
        tramite.ModificarContenido(new ContenidoTramite(request.NuevoContenido), request.UsuarioId, fechaActual);

        // 4. Persistencia
        repo.Modificar(tramite);

        // 5. Orquestación: Actualizar el estado del expediente por si el trámite cambió
        servicioEstado.Actualizar(tramite.ExpedienteId, request.UsuarioId, fechaActual);

        // 6. Retorno de DTO
        return new ModificarTramiteResponse(true);
    }
}