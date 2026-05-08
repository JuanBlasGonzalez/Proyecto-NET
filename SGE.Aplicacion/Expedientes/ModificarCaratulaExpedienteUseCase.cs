namespace SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Autorizacion;

public class ModificarCaratulaExpedienteUseCase(IExpedienteRepository repo, IAutorizacionService auth)
{
    public void Ejecutar(ModificarCaratulaRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.ExpedienteModificacion))
            throw new AutorizacionException("No tiene permisos para modificar carátulas.");

        var expediente = repo.ObtenerPorId(request.IdExpediente) 
            ?? throw new Exception("Expediente no encontrado.");

        // Usamos el método de la entidad que definimos en Dominio
        expediente.ModificarCaratula(new SGE.Dominio.Expedientes.Caratula(request.NuevaCaratula), request.UsuarioId);
        repo.Modificar(expediente);
    }
}