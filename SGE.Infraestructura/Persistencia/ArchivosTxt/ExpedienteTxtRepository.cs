using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.ExceptionApp;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Comun; 
using System.Text.Json; // Para la serialización de los expedientes en el archivo de texto

namespace SGE.Infraestructura.Persistencia;

// Esta clase implementa la interfaz IExpedienteRepository utilizando un archivo de texto para almacenar los datos de los expedientes.
public class ExpedienteTxtRepository : IExpedienteRepository
{
    private readonly string _path = "expedientes.txt";

    public ExpedienteTxtRepository()
    {
        if (!File.Exists(_path)) File.Create(_path).Close();
    }

    public void Agregar(Expediente expediente)
    {
        var lista = ObtenerTodos().ToList();
        lista.Add(expediente);
        GuardarTodos(lista);
    }

    public Expediente? ObtenerPorId(Guid id)
    {
        return ObtenerTodos().FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<Expediente> ObtenerTodos()
    {
        var lineas = File.ReadAllLines(_path);
        foreach (var linea in lineas)
        {
            if (string.IsNullOrWhiteSpace(linea)) continue;

            var dto = JsonSerializer.Deserialize<ExpedienteEstructuralDto>(linea);
            if (dto != null)
            {
                // Reconstruimos el Value Object Caratula pasándole el texto guardado
                var caratulaVO = new Caratula(dto.CaratulaTexto);

                // Resucitamos el Expediente usando su Factory Method
                yield return Expediente.Reconstruir(
                    dto.Id, 
                    caratulaVO, 
                    dto.FechaCreacion, 
                    dto.FechaUltimaModificacion, 
                    dto.UsuarioUltimoCambio, 
                    dto.Estado
                );
            }
        }
    }

    public void Modificar(Expediente expediente)
    {
        var lista = ObtenerTodos().ToList();
        var index = lista.FindIndex(e => e.Id == expediente.Id);

        if (index == -1) 
            throw new RepositorioException("No se encontró el expediente para modificar.");

        lista[index] = expediente;
        GuardarTodos(lista);
    }

    public void Eliminar(Guid id)
    {
        var lista = ObtenerTodos().ToList();
        var expediente = lista.FirstOrDefault(e => e.Id == id);

        if (expediente == null) 
            throw new RepositorioException("No se encontró el expediente para eliminar.");

        lista.Remove(expediente);
        GuardarTodos(lista);
    }

    private void GuardarTodos(IEnumerable<Expediente> expedientes)
    {
        var lineas = expedientes.Select(e => JsonSerializer.Serialize(new ExpedienteEstructuralDto 
        {
            Id = e.Id,
            CaratulaTexto = e.Caratula.ToString(), // Extraemos el texto puro del Value Object
            FechaCreacion = e.FechaCreacion,
            FechaUltimaModificacion = e.FechaUltimaModificacion,
            UsuarioUltimoCambio = e.UsuarioUltimoCambio,
            Estado = e.Estado
        }));
        
        File.WriteAllLines(_path, lineas);
    }
}

internal class ExpedienteEstructuralDto
{
    public Guid Id { get; set; }
    public string CaratulaTexto { get; set; } = "";
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaUltimaModificacion { get; set; }
    public Guid UsuarioUltimoCambio { get; set; }
    public EstadoExpediente Estado { get; set; }
}