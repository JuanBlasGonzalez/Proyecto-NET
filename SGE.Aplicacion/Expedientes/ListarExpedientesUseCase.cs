namespace SGE.Aplicacion.Expedientes;

public class ListarExpedientesUseCase(IExpedienteRepository repo)
{
    public IEnumerable<ExpedienteDTO> Ejecutar()
    {
        var expedientes = repo.ObtenerTodos();
        
        // Mapeamos de Entidad a DTO para que no "escapen" las entidades de la capa
        return expedientes.Select(e => new ExpedienteDTO(
            e.Id, 
            e.Caratula.Valor, 
            e.Estado.ToString(), 
            e.FechaUltimaModificacion // Asegurate que este nombre coincida con tu entidad
        ));
    }
}