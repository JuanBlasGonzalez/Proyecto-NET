using SGE.Aplicacion.Expedientes;

namespace SGE.Aplicacion.Tramites;

public class ActualizacionEstadoExpedienteService(IExpedienteRepository repoExp, ITramiteRepository repoTram)
{
    public void Actualizar(Guid expedienteId, Guid usuarioId)
    {
        var expediente = repoExp.ObtenerPorId(expedienteId) 
            ?? throw new Exception("Expediente no encontrado.");

        // Buscamos el último trámite (el de fecha más reciente)
        var ultimoTramite = repoTram.ObtenerPorExpedienteId(expedienteId)
            .OrderByDescending(t => t.FechaCreacion)
            .FirstOrDefault();

        // Si no hay trámites, el estado debería ser el inicial (o lo que defina tu lógica)
        if (ultimoTramite != null)
        {
            // La entidad decide si cambia el estado. Solo guardamos si hubo cambio (bool true)
            if (expediente.ActualizarEstado(ultimoTramite.Etiqueta, usuarioId))
            {
                repoExp.Modificar(expediente);
            }
        }
    }
}