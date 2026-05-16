using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.ExceptionApp;
using SGE.Aplicacion.Fecha;

namespace SGE.Aplicacion.Tramites;
public class BajaTramiteUseCase(ITramiteRepository repo, IAutorizacionService auth, ActualizacionEstadoExpedienteService servicioEstado, IDateTimeProvider dateTimeProvider)
{
    public void Ejecutar(Guid tramiteId, Guid usuarioId)
    {
        if (!auth.PoseeElPermiso(usuarioId, Permiso.TramiteBaja))
            throw new AutorizacionException("No tiene permisos.");

        var tramite = repo.ObtenerPorId(tramiteId) ?? throw new Exception("No existe");
        var fechaActual = dateTimeProvider.ObtenerFechaActual();
        repo.Eliminar(tramiteId);

        //Al borrarlo, el expediente debe recalcular su estado
        servicioEstado.Actualizar(tramite.ExpedienteId, usuarioId, fechaActual);
    }
}