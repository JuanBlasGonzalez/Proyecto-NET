namespace SGE.Aplicacion.Expedientes;

using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Tramites;
public class BajaExpedienteUseCase(IExpedienteRepository repoExp, ITramiteRepository repoTram, IAutorizacionService auth)
{
    public void Ejecutar(BajaExpedienteRequest request)
    {
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.ExpedienteBaja))
            throw new AutorizacionException("Sin permisos de Baja.");

        // 1. ELIMINACIÓN EN CASCADA
        var tramites = repoTram.ObtenerPorExpedienteId(request.IdExpediente);
        foreach (var t in tramites)
        {
            repoTram.Eliminar(t.Id);
        }

        // 2. ELIMINAR EXPEDIENTE
        repoExp.Eliminar(request.IdExpediente);
    }
}