using SGE.Aplicacion.Expedientes; // Para usar IExpedienteRepository
using SGE.Dominio.Tramites;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Autorizacion;

namespace SGE.Aplicacion.Tramites;

public record AltaTramiteRequest(Guid ExpedienteId, string Contenido, SGE.Dominio.Tramites.EtiquetaTramite Etiqueta, Guid UsuarioId);

public class AltaTramiteUseCase(
    ITramiteRepository repoTram, 
    IAutorizacionService auth, 
    ActualizacionEstadoExpedienteService servicioEstado)
{
    public void Ejecutar(AltaTramiteRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.TramiteAlta))
            throw new AutorizacionException("Sin permisos.");

        var tramite = new SGE.Dominio.Tramites.Tramite(
            request.ExpedienteId, 
            request.Etiqueta, 
            new SGE.Dominio.Tramites.ContenidoTramite(request.Contenido), 
            request.UsuarioId);

        repoTram.Agregar(tramite);

        // Delegamos la actualización del estado al servicio orquestador
        servicioEstado.Actualizar(request.ExpedienteId, request.UsuarioId);
    }
}