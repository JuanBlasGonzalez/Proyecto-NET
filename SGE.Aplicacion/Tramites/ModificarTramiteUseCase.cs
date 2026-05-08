namespace SGE.Aplicacion.Tramites;

using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Tramites;

public class ModificarTramiteUseCase(
    ITramiteRepository repo,
    IAutorizacionService auth,
    ActualizacionEstadoExpedienteService servicioEstado)
{
    public void Ejecutar(ModificarTramiteRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.TramiteModificacion))
            throw new AutorizacionException("No tiene permisos para modificar trámites.");

        var tramite = repo.ObtenerPorId(request.IdTramite) ?? throw new Exception("Trámite no encontrado.");

        tramite.ModificarContenido(new ContenidoTramite(request.NuevoContenido),request.UsuarioId);

        repo.Modificar(tramite);

        servicioEstado.Actualizar(tramite.ExpedienteId, request.UsuarioId);
    }
}