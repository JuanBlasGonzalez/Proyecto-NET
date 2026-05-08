using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class AltaExpedienteUseCase(IExpedienteRepository repo, IAutorizacionService auth)
{
    public AltaExpedienteResponse Ejecutar(AltaExpedienteRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.ExpedienteAlta))
            throw new AutorizacionException("Sin permisos de Alta.");

        var expediente = new SGE.Dominio.Expedientes.Expediente(
            new SGE.Dominio.Expedientes.Caratula(request.Caratula), request.UsuarioId);
        
        repo.Agregar(expediente);
        return new AltaExpedienteResponse(expediente.Id, true);
    }
}