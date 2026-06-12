namespace SGE.Aplicacion.Tramites;

public class ListarTramitesPorExpedienteUseCase(ITramiteRepository repo)
{
    public ListarTramitesResponse Ejecutar(ListarTramitesRequest request)
    {
        var tramites = repo.ObtenerPorExpedienteId(request.ExpedienteId);
        
        var dtos = tramites.Select(t => new TramiteDTO(
            t.Id,
            t.ExpedienteId,
            t.Etiqueta, 
            t.Contenido.Valor,
            t.FechaCreacion,
            t.FechaUltimaModificacion,
            t.UsuarioUltimoCambio
        )).ToList();

        return new ListarTramitesResponse(dtos);
    }
}