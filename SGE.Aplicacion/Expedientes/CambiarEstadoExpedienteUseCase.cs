using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Fecha;

namespace SGE.Aplicacion.Expedientes;

public class CambiarEstadoExpedienteUseCase(IExpedienteRepository repo, IAutorizacionService auth, IDateTimeProvider timeProvider)
{
    public CambiarEstadoResponse Ejecutar(CambiarEstadoRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.ExpedienteModificacion))
            throw new AutorizacionException("Sin permisos.");

        var expediente = repo.ObtenerPorId(request.IdExpediente) 
            ?? throw new Exception("Expediente no encontrado.");

        var fechaActual = timeProvider.ObtenerFechaActual();
        // Se pasa la fecha actual para que el expediente registre cuándo cambió de estado
        expediente.CambiarEstado(request.NuevoEstado, request.UsuarioId, fechaActual);

        repo.Modificar(expediente);
        return new CambiarEstadoResponse(true);
    }
}