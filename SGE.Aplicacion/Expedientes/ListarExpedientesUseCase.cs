namespace SGE.Aplicacion.Expedientes;

// Esta clase representa el caso de uso para listar todos los expedientes existentes en la aplicación.
// El constructor de la clase toma una dependencia: un repositorio de expedientes (IExpedienteRepository), que se utiliza para acceder a los datos de los expedientes almacenados en la aplicación. 
public class ListarExpedientesUseCase(IExpedienteRepository repo)
{
    public IEnumerable<ExpedienteDTO> Ejecutar()
    {
        var expedientes = repo.ObtenerTodos();
        
        return expedientes.Select(e => new ExpedienteDTO(
            e.Id, 
            e.Caratula.Valor, 
            e.Estado.ToString(), 
            e.FechaUltimaModificacion 
        ));
    }
}