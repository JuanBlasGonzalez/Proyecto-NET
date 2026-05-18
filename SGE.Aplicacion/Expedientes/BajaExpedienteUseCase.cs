namespace SGE.Aplicacion.Expedientes;

using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.ExceptionApp;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Fecha;

// Esta clase representa el caso de uso para la eliminación de un expediente existente.
// El constructor de la clase toma tres dependencias: un repositorio de expedientes (IExpedienteRepository), un repositorio de trámites (ITramiteRepository) y un servicio de autorización (IAutorizacionService).
public class BajaExpedienteUseCase(IExpedienteRepository repoExp, ITramiteRepository repoTram, IAutorizacionService auth)
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
        // Buscamos los trámites asociados antes de borrar el expediente
        var tramites = repoTram.ObtenerPorExpedienteId(request.IdExpediente);
        foreach (var t in tramites)
        {
            repoTram.Eliminar(t.Id);
        }

        // 3. ELIMINAR EXPEDIENTE
        repoExp.Eliminar(request.IdExpediente);

        // 4. Retornar el DTO de respuesta
        return new BajaExpedienteResponse(true, "Expediente y sus trámites asociados fueron eliminados.");
    }
}