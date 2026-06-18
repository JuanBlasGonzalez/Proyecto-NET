using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Fecha;
using SGE.Dominio.Usuarios;
using SGE.Aplicacion.Interfaces;

namespace SGE.Aplicacion.Tramites;

public class AltaTramiteUseCase(
    ITramiteRepository repoTram, 
    IAutorizacionService auth, 
    ActualizacionEstadoExpedienteService servicioEstado,
    IDateTimeProvider dateTimeProvider,
    IUnidadDeTrabajo uow) 
{
    public AltaTramiteResponse Ejecutar(AltaTramiteRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.TramiteAlta))
            throw new AutorizacionException("Sin permisos para dar de alta un trámite.");

        var fechaActual = dateTimeProvider.ObtenerFechaActual();

        var tramite = new Tramite(
            request.ExpedienteId, 
            request.Etiqueta, 
            new ContenidoTramite(request.Contenido), 
            request.UsuarioId,
            fechaActual);

        repoTram.Agregar(tramite); // Marca en memoria

        servicioEstado.Actualizar(request.ExpedienteId, request.UsuarioId, fechaActual); // Modifica expediente en memoria si corresponde

        uow.Guardar(); // LA REGLA DE ORO: Impacta el trámite nuevo y el cambio de estado del expediente de forma atómica

        return new AltaTramiteResponse(tramite.Id, true);
    }
}