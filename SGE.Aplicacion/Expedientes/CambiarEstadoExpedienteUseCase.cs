using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Fecha;
using SGE.Dominio.Usuarios;
using SGE.Aplicacion.Interfaces;

namespace SGE.Aplicacion.Expedientes;

public class CambiarEstadoExpedienteUseCase(IExpedienteRepository repo, IAutorizacionService auth, IDateTimeProvider timeProvider, IUnidadDeTrabajo uow)
{
    public CambiarEstadoResponse Ejecutar(CambiarEstadoRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.ExpedienteModificacion))
            throw new AutorizacionException("Sin permisos.");

        var expediente = repo.ObtenerPorId(request.IdExpediente) 
            ?? throw new Exception("Expediente no encontrado.");

        var fechaActual = timeProvider.ObtenerFechaActual();
        expediente.CambiarEstado(request.NuevoEstado, request.UsuarioId, fechaActual);

        repo.Modificar(expediente); // Marca modificación en memoria
        
        uow.Guardar(); // LA REGLA DE ORO

        return new CambiarEstadoResponse(true);
    }
}