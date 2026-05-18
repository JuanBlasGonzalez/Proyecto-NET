namespace SGE.Aplicacion.Tramites;

// Esta clase es un DTO (Data Transfer Object) que se utiliza para transferir datos relacionados con un trámite entre diferentes capas de la aplicación.
public class TramiteDTO
{
    public Guid Id { get; set; }
    public Guid ExpedienteId { get; set; }
    public string Etiqueta { get; set; } = "";
    public string Contenido { get; set; } = "";
    public DateTime FechaCreacion { get; set; }
}