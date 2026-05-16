namespace SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Fecha;

public class ModificarCaratulaExpedienteUseCase(IExpedienteRepository repo, IAutorizacionService auth, IDateTimeProvider timeProvider)
{
   public ModificarCaratulaResponse Ejecutar(ModificarCaratulaRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.ExpedienteModificacion))
            throw new AutorizacionException("Sin permisos.");

        var expediente = repo.ObtenerPorId(request.IdExpediente) 
            ?? throw new Exception("Expediente no encontrado.");

        var fechaActual = timeProvider.ObtenerFechaActual();
        // Usamos el Value Object para validar la nueva carátula
        expediente.ModificarCaratula(new SGE.Dominio.Expedientes.Caratula(request.NuevaCaratula), request.UsuarioId, fechaActual);

        repo.Modificar(expediente);
        return new ModificarCaratulaResponse(true);
    }
}