using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Usuarios;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Fecha;
using SGE.Aplicacion.Interfaces; // Asegurá que acá viva IUnidadDeTrabajo

namespace SGE.Aplicacion.Expedientes;

public class AltaExpedienteUseCase(IExpedienteRepository repo, IAutorizacionService auth, IDateTimeProvider timeProvider, IUnidadDeTrabajo uow)
{
    public AltaExpedienteResponse Ejecutar(AltaExpedienteRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.ExpedienteAlta))
            throw new AutorizacionException("Sin permisos de Alta.");

        DateTime fechaActual = timeProvider.ObtenerFechaActual();
       
        var expediente = new SGE.Dominio.Expedientes.Expediente(
            new SGE.Dominio.Expedientes.Caratula(request.Caratula), request.UsuarioId, fechaActual);
        
        repo.Agregar(expediente); // Marca la entidad en memoria
        
        uow.Guardar(); // LA REGLA DE ORO: Confirma los cambios de forma atómica
        
        return new AltaExpedienteResponse(expediente.Id, true);
    }
}