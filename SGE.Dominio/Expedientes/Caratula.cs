namespace SGE.Dominio.Expedientes;
using SGE.Dominio.Comun;

// Esta clase representa la carátula de un expediente, que es un valor que no puede ser modificado después de su creación.
// La clausula Record class se utiliza para definir una clase inmutable que se comporta como un valor, 
//lo que significa que dos instancias de Caratula con el mismo valor serán consideradas iguales.
public record class Caratula
{
    public string Valor { get; }

    public Caratula(string valor)
    {
        // Regla: No puede ser nulo ni vacío
        if (string.IsNullOrWhiteSpace(valor))
        {
           throw new DominioException("La carátula no puede estar vacía.");
        }
        Valor = valor;
    }
}

