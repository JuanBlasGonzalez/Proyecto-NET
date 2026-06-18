using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Fecha;
using SGE.Dominio.Usuarios;
using SGE.Aplicacion.Interfaces;

namespace SGE.Aplicacion.Tramites;

public class BajaTramiteUseCase(
    ITramiteRepository repo, 
    IAutorizacionService auth, 
    ActualizacionEstadoExpedienteService servicioEstado, 
    IDateTimeProvider dateTimeProvider,
    IUnidadDeTrabajo uow)
{
    public BajaTramiteResponse Ejecutar(BajaTramiteRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.TramiteBaja))
            throw new AutorizacionException("No tiene permisos.");

        var tramite = repo.ObtenerPorId(request.TramiteId) 
            ?? throw new Exception("No existe el trámite especificado.");
            
        var fechaActual = dateTimeProvider.ObtenerFechaActual();
        repo.Eliminar(request.TramiteId); // Marca eliminación en memoria

        servicioEstado.Actualizar(tramite.ExpedienteId, request.UsuarioId, fechaActual); // Recalcula y marca el expediente en memoria

        uow.Guardar(); // LA REGLA DE ORO

        return new BajaTramiteResponse(true, "Trámite eliminado exitosamente y estado de expediente recalculado.");
    }
}