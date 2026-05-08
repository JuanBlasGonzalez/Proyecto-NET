namespace SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Autorizacion;


public class ModificarTramiteUseCase(ITramiteRepository repo, IAutorizacionService auth, ActualizacionEstadoExpedienteService servicioEstado)
{
    public void Ejecutar(ModificarTramiteRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.TramiteModificacion))
            throw new AutorizacionException("No tiene permisos para modificar trámites.");

        var tramite = repo.ObtenerPorId(request.IdTramite) ?? throw new Exception("Trámite no encontrado.");
        
        // Asumo que en tu Dominio de Trámite el método es ModificarContenido
        tramite.ModificarContenido(new SGE.Dominio.Tramites.ContenidoTramite(request.NuevoContenido), request.UsuarioId);
        repo.Modificar(tramite);

        // Al modificar un trámite, refrescamos el estado del expediente por si cambió algo
        servicioEstado.Actualizar(tramite.ExpedienteId, request.UsuarioId);
    }
}