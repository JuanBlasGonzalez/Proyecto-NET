using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Fecha;

namespace SGE.Aplicacion.Expedientes;
// Esta clase representa el caso de uso para la creación de un nuevo expediente.
// El constructor de la clase toma tres dependencias: un repositorio de expedientes (IExpedienteRepository), un servicio de autorización (IAutorizacionService) y un proveedor de fecha y hora (IDateTimeProvider).
public class AltaExpedienteUseCase(IExpedienteRepository repo, IAutorizacionService auth, IDateTimeProvider timeProvider)
{
    public AltaExpedienteResponse Ejecutar(AltaExpedienteRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.ExpedienteAlta))
            throw new AutorizacionException("Sin permisos de Alta.");

        DateTime fechaActual = timeProvider.ObtenerFechaActual();
       
        var expediente = new SGE.Dominio.Expedientes.Expediente(
            new SGE.Dominio.Expedientes.Caratula(request.Caratula), request.UsuarioId, fechaActual);
        
        repo.Agregar(expediente);
        return new AltaExpedienteResponse(expediente.Id, true);
    }
}