using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Fecha;


namespace SGE.Aplicacion.Tramites;

// Esta clase representa el caso de uso para dar de alta un nuevo trámite en un expediente existente.
public class AltaTramiteUseCase(
    ITramiteRepository repoTram, 
    IAutorizacionService auth, 
    ActualizacionEstadoExpedienteService servicioEstado,
    IDateTimeProvider dateTimeProvider) 
{
    // 1. Cambiamos el tipo de retorno de void a AltaTramiteResponse
    public AltaTramiteResponse Ejecutar(AltaTramiteRequest request)
    {
        // 2. Validación de permisos
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.TramiteAlta))
            throw new AutorizacionException("Sin permisos para dar de alta un trámite.");

        var fechaActual = dateTimeProvider.ObtenerFechaActual();

        // 3. Creación de la entidad
        var tramite = new Tramite(
            request.ExpedienteId, 
            request.Etiqueta, 
            new ContenidoTramite(request.Contenido), 
            request.UsuarioId,
            fechaActual);

        // 4. Persistencia
        repoTram.Agregar(tramite);

        // 5. Actualización del estado del expediente en función del nuevo trámite registrado
        servicioEstado.Actualizar(request.ExpedienteId, request.UsuarioId, fechaActual);

        // 6. Retornamos el Response con el ID generado
        return new AltaTramiteResponse(tramite.Id, true);
    }
}