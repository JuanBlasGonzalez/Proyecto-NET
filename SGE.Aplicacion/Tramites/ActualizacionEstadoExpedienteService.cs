using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.ExceptionApp;
namespace SGE.Aplicacion.Tramites;

// Esta clase representa un servicio que se encarga de actualizar el estado de un expediente en función del último trámite registrado para ese expediente.
// El constructor de la clase toma dos dependencias: un repositorio de expedientes (IExpedienteRepository) y un repositorio de trámites (ITramiteRepository), que se utilizan para acceder a los datos de los expedientes y trámites almacenados en la aplicación.   
public class ActualizacionEstadoExpedienteService(IExpedienteRepository repoExp, ITramiteRepository repoTram)
{
    public void Actualizar(Guid expedienteId, Guid usuarioId, DateTime fechaActual)
    {
        var expediente = repoExp.ObtenerPorId(expedienteId) 
            ?? throw new RepositorioException("Expediente no encontrado.");

        // Buscamos el último trámite (el de fecha más reciente)
        var ultimoTramite = repoTram.ObtenerPorExpedienteId(expedienteId).OrderByDescending(t => t.FechaCreacion).FirstOrDefault();

        // Si no hay trámites, el estado debería ser el inicial
        if (ultimoTramite != null)
        {
            // La entidad decide si cambia el estado. Solo guardamos si hubo cambio (bool true)
            if (expediente.ActualizarEstado(ultimoTramite.Etiqueta, usuarioId, fechaActual))
            {
                repoExp.Modificar(expediente);
            }
        }
    }
}