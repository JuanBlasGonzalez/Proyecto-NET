namespace SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Autorizacion;

public class ModificarCaratulaExpedienteUseCase(IExpedienteRepository repo, IAutorizacionService auth)
{
   public ModificarCaratulaResponse Ejecutar(ModificarCaratulaRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.ExpedienteModificacion))
            throw new AutorizacionException("Sin permisos.");

        var expediente = repo.ObtenerPorId(request.IdExpediente) 
            ?? throw new Exception("Expediente no encontrado.");

        // Usamos el Value Object para validar la nueva carátula
        expediente.ModificarCaratula(new SGE.Dominio.Expedientes.Caratula(request.NuevaCaratula), request.UsuarioId);

        repo.Modificar(expediente);
        return new ModificarCaratulaResponse(true);
    }
}