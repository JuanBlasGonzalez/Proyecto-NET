using SGE.Dominio.Usuarios;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.ExceptionApp;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Fecha;
using SGE.Aplicacion.Interfaces;

namespace SGE.Aplicacion.Expedientes;

public class BajaExpedienteUseCase(IExpedienteRepository repoExp, ITramiteRepository repoTram, IAutorizacionService auth, IUnidadDeTrabajo uow)
{
    public BajaExpedienteResponse Ejecutar(BajaExpedienteRequest request)
    {
        // 1. Validar permisos
        if (!auth.PoseeElPermiso(request.UsuarioId, Permiso.ExpedienteBaja))
            throw new AutorizacionException("Sin permisos de Baja.");

        // 2. Validar existencia del expediente
        var existe = repoExp.ObtenerPorId(request.IdExpediente);
        if (existe == null) throw new RepositorioException("El expediente a eliminar no existe.");
        
        // 3. ELIMINACIÓN EN CASCADA
        var tramites = repoTram.ObtenerPorExpedienteId(request.IdExpediente);
        foreach (var t in tramites)
        {
            repoTram.Eliminar(t.Id); // Marca eliminación de trámites en memoria
        }

        // 4. ELIMINAR EXPEDIENTE
        repoExp.Eliminar(request.IdExpediente); // Marca eliminación del expediente en memoria

        uow.Guardar(); // LA REGLA DE ORO: Impacta el borrado en cascada entero en un único commit a SQLite

        return new BajaExpedienteResponse(true, "Expediente y sus trámites asociados fueron eliminados.");
    }
}