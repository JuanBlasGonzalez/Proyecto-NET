namespace SGE.Aplicacion.ExceptionApp;

// Esta clase hereda de Exception y se utiliza para representar excepciones específicas relacionadas con los repositorios en la aplicación.
public class RepositorioException(string mensaje) : Exception(mensaje);