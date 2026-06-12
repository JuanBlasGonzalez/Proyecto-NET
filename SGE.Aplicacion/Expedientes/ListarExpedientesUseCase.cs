namespace SGE.Aplicacion.Expedientes;

// Esta clase representa el caso de uso para listar todos los expedientes existentes en la aplicación.
// El constructor de la clase toma una dependencia: un repositorio de expedientes (IExpedienteRepository), que se utiliza para acceder a los datos de los expedientes almacenados en la aplicación. 
public class ListarExpedientesUseCase(IExpedienteRepository repo)
{
    // NUEVO: Implementamos logica de request y response.
    public ListarExpedientesResponse Ejecutar(ListarExpedientesRequest request)
    {
        var expedientes = repo.ObtenerTodos();
        
        // Mapeamos el dominio al nuevo DTO con campos completos y tipado fuerte
        var dtos = expedientes.Select(e => new ExpedienteDTO(
            e.Id, 
            e.Caratula.Valor, 
            e.FechaCreacion,
            e.FechaUltimaModificacion,
            e.UsuarioUltimoCambio,
            e.Estado
        )).ToList(); // Lo materializamos para pasarlo seguro al Response

        return new ListarExpedientesResponse(dtos);
    }
}