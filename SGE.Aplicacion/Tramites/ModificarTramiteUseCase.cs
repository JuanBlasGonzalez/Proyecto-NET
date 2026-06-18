using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.ExceptionApp;
using SGE.Aplicacion.Fecha;
using SGE.Dominio.Usuarios;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Interfaces;

namespace SGE.Aplicacion.Tramites;

public class ModificarTramiteUseCase(
    ITramiteRepository repo,
    IAutorizacionService auth,
    ActualizacionEstadoExpedienteService servicioEstado,
    IDateTimeProvider timeProvider,
    IUnidadDeTrabajo uow)
{
    public ModificarTramiteResponse Ejecutar(ModificarTramiteRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.TramiteModificacion))
            throw new AutorizacionException("No tiene permisos para modificar trámites.");

        var tramite = repo.ObtenerPorId(request.IdTramite) 
            ?? throw new RepositorioException("Trámite no encontrado.");

        var fechaActual = timeProvider.ObtenerFechaActual();
        tramite.ModificarContenido(new ContenidoTramite(request.NuevoContenido), request.UsuarioId, fechaActual);

        repo.Modificar(tramite); // Marca modificación en memoria

        servicioEstado.Actualizar(tramite.ExpedienteId, request.UsuarioId, fechaActual); // Recalcula y marca el expediente en memoria

        uow.Guardar(); // LA REGLA DE ORO

        return new ModificarTramiteResponse(true);
    }
}