namespace SGE.Dominio.Tramites;
using SGE.Dominio.Comun;

// Esta clase representa el contenido de un trámite, que es un valor que no puede ser modificado después de su creación.
public record class ContenidoTramite
{
    public string Valor { get; init; }

    public ContenidoTramite(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new DominioException("El contenido del trámite no puede estar vacío.");
        }
        Valor = valor;
    }
}