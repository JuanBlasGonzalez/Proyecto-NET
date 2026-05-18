namespace SGE.Aplicacion.Tramites;

// Esta clase representa el caso de uso para listar los trámites asociados a un expediente específico.
public class ListarTramitesPorExpedienteUseCase(ITramiteRepository repo)
{
    public IEnumerable<TramiteDTO> Ejecutar(Guid expedienteId)
    {
        var tramites = repo.ObtenerPorExpedienteId(expedienteId);
        
        return tramites.Select(t => new TramiteDTO {
            Id = t.Id,
            ExpedienteId = t.ExpedienteId,
            Etiqueta = t.Etiqueta.ToString(),
            Contenido = t.Contenido.Valor, // Usamos .Valor como en la carátula
            FechaCreacion = t.FechaCreacion
        });
    }
}