namespace SGE.Dominio.Comun;

// Esta clase hereda de Exception para que .NET la reconozca como un error válido
public class DominioException : Exception
{
    public DominioException(string mensaje) : base(mensaje) { }
}