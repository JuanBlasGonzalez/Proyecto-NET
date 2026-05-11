using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class CambiarEstadoExpedienteUseCase(IExpedienteRepository repo, IAutorizacionService auth)
{
    public CambiarEstadoResponse Ejecutar(CambiarEstadoRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.ExpedienteModificacion))
            throw new AutorizacionException("Sin permisos.");

        var expediente = repo.ObtenerPorId(request.IdExpediente) 
            ?? throw new Exception("Expediente no encontrado.");

        // Este es el cambio MANUAL pedido por el enunciado
        expediente.CambiarEstado(request.NuevoEstado, request.UsuarioId);

        repo.Modificar(expediente);
        return new CambiarEstadoResponse(true);
    }
}