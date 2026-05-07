using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Comun;

try
{
    Console.WriteLine("--- Testeando Creación de Expediente ---");
    var miUsuarioId = Guid.NewGuid();
    
    // 1. Creamos la carátula y el expediente
    var caratulaInicial = new Caratula("Solicitud de Becas 2026");
    var exp = new Expediente(caratulaInicial, miUsuarioId);
    
    Console.WriteLine($"Expediente creado: {exp.Id}");
    Console.WriteLine($"Estado inicial: {exp.Estado}"); // Debería ser RecienIniciado

    Console.WriteLine("\n--- Testeando Cambio de Estado Automático ---");
    
    // 2. Simulamos que llega un trámite de "Pase a Estudio"
    // (Solo usamos la etiqueta para probar la lógica del expediente)
    exp.ActualizarEstado(EtiquetaTramite.PaseAEstudio, miUsuarioId);
    Console.WriteLine($"Nuevo estado tras PaseAEstudio: {exp.Estado}"); // Debería ser ParaResolver

    Console.WriteLine("\n--- Testeando Validaciones (Debería fallar) ---");
    // 3. Intentamos crear una carátula vacía para ver si salta nuestra DominioException
    var caratulaMala = new Caratula(""); 
}
catch (DominioException ex)
{
    Console.WriteLine($"Capturamos un error de negocio: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error inesperado: {ex.Message}");
}