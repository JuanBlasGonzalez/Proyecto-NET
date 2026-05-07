namespace SGE.Dominio.Expedientes;
using SGE.Dominio.Comun;

//Record class es una clase que proveee inmutabilidad e igualdad estructural --> Value Objetc
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

