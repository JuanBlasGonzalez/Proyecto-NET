using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.ExceptionApp;
using SGE.Dominio.Tramites;
using System.Text.Json; // Para la serialización de los trámites en el archivo de texto

namespace SGE.Infraestructura.Persistencia;

// Esta clase implementa la interfaz ITramiteRepository utilizando un archivo de texto para almacenar los datos de los trámites.
public class TramiteTxtRepository : ITramiteRepository
{
    private readonly string _path = "tramites.txt";

    public TramiteTxtRepository()
    {
        if (!File.Exists(_path)) File.Create(_path).Close();
    }

    public void Agregar(Tramite tramite)
    {
        var lista = ObtenerTodosLosTramitesFisicos();
        lista.Add(tramite);
        GuardarTodos(lista);
    }

    public Tramite? ObtenerPorId(Guid id)
    {
        return ObtenerTodosLosTramitesFisicos().FirstOrDefault(t => t.Id == id);
    }

    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId)
    {
        return ObtenerTodosLosTramitesFisicos().Where(t => t.ExpedienteId == expedienteId);
    }

    public void Modificar(Tramite tramite)
    {
        var lista = ObtenerTodosLosTramitesFisicos();
        var index = lista.FindIndex(t => t.Id == tramite.Id);

        if (index == -1) 
            throw new RepositorioException("No se encontró el trámite para modificar.");

        lista[index] = tramite;
        GuardarTodos(lista);
    }

    public void Eliminar(Guid id)
    {
        var lista = ObtenerTodosLosTramitesFisicos();
        var tramite = lista.FirstOrDefault(t => t.Id == id);

        if (tramite == null) 
            throw new RepositorioException("No se encontró el trámite para eliminar.");

        lista.Remove(tramite);
        GuardarTodos(lista);
    }

    private List<Tramite> ObtenerTodosLosTramitesFisicos()
    {
        var lista = new List<Tramite>();
        var lineas = File.ReadAllLines(_path);

        foreach (var linea in lineas)
        {
            if (string.IsNullOrWhiteSpace(linea)) continue;

            var dto = JsonSerializer.Deserialize<TramiteEstructuralDto>(linea);
            if (dto != null)
            {
                // Reconstruimos el Value Object ContenidoTramite pasándole el texto guardado
                var contenidoVO = new ContenidoTramite(dto.ContenidoTexto);

                // Resucitamos el Trámite con su Factory Method
                var tramiteReconstruido = Tramite.Reconstruir(
                    dto.Id, 
                    dto.ExpedienteId, 
                    dto.Etiqueta, 
                    contenidoVO, 
                    dto.FechaCreacion, 
                    dto.FechaUltimaModificacion, 
                    dto.UsuarioUltimoCambio
                );
                lista.Add(tramiteReconstruido);
            }
        }
        return lista;
    }

    private void GuardarTodos(IEnumerable<Tramite> tramites)
    {
        var lineas = tramites.Select(t => JsonSerializer.Serialize(new TramiteEstructuralDto 
        {
            Id = t.Id,
            ExpedienteId = t.ExpedienteId,
            Etiqueta = t.Etiqueta,
            ContenidoTexto = t.Contenido.ToString(), // Extraemos el texto del Value Object
            FechaCreacion = t.FechaCreacion,
            FechaUltimaModificacion = t.FechaUltimaModificacion,
            UsuarioUltimoCambio = t.UsuarioUltimoCambio
        }));
        
        File.WriteAllLines(_path, lineas);
    }
}

internal class TramiteEstructuralDto
{
    public Guid Id { get; set; }
    public Guid ExpedienteId { get; set; }
    public EtiquetaTramite Etiqueta { get; set; }
    public string ContenidoTexto { get; set; } = "";
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaUltimaModificacion { get; set; }
    public Guid UsuarioUltimoCambio { get; set; }
}