using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class CambiarEstadoExpedienteUseCase(IExpedienteRepository repo, IAutorizacionService auth)
{
    public void Ejecutar(CambiarEstadoRequest request)
    {
        // 1. Validar permisos (Usamos el de modificación)
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.ExpedienteModificacion))
            throw new AutorizacionException("No tiene permisos para cambiar el estado del expediente.");

        // 2. Buscar el expediente
        var expediente = repo.ObtenerPorId(request.IdExpediente) 
            ?? throw new Exception("Expediente no encontrado.");

        // 3. Cambiar el estado usando el método del Dominio
        // (Asegurate que en tu clase Expediente el método se llame CambiarEstado)
        expediente.CambiarEstado(request.NuevoEstado, request.UsuarioId);

        // 4. Persistir el cambio
        repo.Modificar(expediente);
    }
}