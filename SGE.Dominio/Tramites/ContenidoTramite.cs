namespace SGE.Dominio.Tramites;
using SGE.Dominio.Comun;

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