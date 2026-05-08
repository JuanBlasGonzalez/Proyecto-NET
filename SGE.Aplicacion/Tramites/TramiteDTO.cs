namespace SGE.Aplicacion.Tramites;

public class TramiteDTO
{
    public Guid Id { get; set; }
    public Guid ExpedienteId { get; set; }
    public string Etiqueta { get; set; } = "";
    public string Contenido { get; set; } = "";
    public DateTime FechaCreacion { get; set; }
}