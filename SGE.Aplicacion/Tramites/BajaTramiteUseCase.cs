using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Fecha;

namespace SGE.Aplicacion.Tramites;

public class BajaTramiteUseCase(ITramiteRepository repo, IAutorizacionService auth, ActualizacionEstadoExpedienteService servicioEstado, IDateTimeProvider dateTimeProvider)
{
    // Ahora recibe el Request corporativo y devuelve el Response correspondiente
    public BajaTramiteResponse Ejecutar(BajaTramiteRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.TramiteBaja))
            throw new AutorizacionException("No tiene permisos.");

        var tramite = repo.ObtenerPorId(request.TramiteId) 
            ?? throw new Exception("No existe el trámite especificado.");
            
        var fechaActual = dateTimeProvider.ObtenerFechaActual();
        repo.Eliminar(request.TramiteId);

        // Al borrarlo, el expediente debe recalcular su estado
        servicioEstado.Actualizar(tramite.ExpedienteId, request.UsuarioId, fechaActual);

        return new BajaTramiteResponse(true, "Trámite eliminado exitosamente y estado de expediente recalculado.");
    }
}