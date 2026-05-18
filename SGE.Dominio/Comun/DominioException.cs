namespace SGE.Dominio.Comun;

// Esta clase hereda de Exception y se utiliza para representar excepciones específicas del dominio de la aplicación.
public class DominioException : Exception
{
    public DominioException(string mensaje) : base(mensaje) { }
}