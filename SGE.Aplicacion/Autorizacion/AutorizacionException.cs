namespace SGE.Aplicacion.Autorizacion;

// Esta clase hereda de Exception y se utiliza para representar excepciones específicas relacionadas con la autorización en la aplicación.
public class AutorizacionException : Exception
{
    public AutorizacionException(string mensaje) : base(mensaje) { }
}