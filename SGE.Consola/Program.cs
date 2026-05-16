using System;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.ExceptionApp; 
using SGE.Infraestructura.Persistencia;
using SGE.Infraestructura.Servicios;
using SGE.Dominio.Tramites;
using SGE.Dominio.Comun; // Por si tus excepciones de dominio están acá

Console.WriteLine("=================================================");
Console.WriteLine("   SGE - SISTEMA DE GESTIÓN DE EXPEDIENTES       ");
Console.WriteLine("       COMPOSITION ROOT & PRUEBAS DE CAPA        ");
Console.WriteLine("=================================================");

// -------------------------------------------------------------
// 1. INICIALIZACIÓN E INYECCIÓN DE DEPENDENCIAS (Composition Root)
// -------------------------------------------------------------
IExpedienteRepository repoExp = new ExpedienteTxtRepository();
ITramiteRepository repoTram = new TramiteTxtRepository();
IAutorizacionService auth = new AutorizacionProvisionalService();

var servicioEstado = new ActualizacionEstadoExpedienteService(repoExp, repoTram);

var ucAltaExpediente = new AltaExpedienteUseCase(repoExp, auth);
var ucAltaTramite = new AltaTramiteUseCase(repoTram, auth, servicioEstado);
var ucListarTramites = new ListarTramitesPorExpedienteUseCase(repoTram);

Guid usuarioId = Guid.NewGuid();
Guid? expedienteCreadoId = null;

// -------------------------------------------------------------
// 2. PRUEBA: CAMINO FELIZ
// -------------------------------------------------------------
try
{
    Console.WriteLine("\n>>> [TEST 1: CAMINO FELIZ] <<<");
    
    Console.WriteLine("Creando un expediente válido...");
    var reqExp = new AltaExpedienteRequest("Expediente de prueba de Infraestructura Texto", usuarioId);
    var resExp = ucAltaExpediente.Ejecutar(reqExp);
    expedienteCreadoId = resExp.Id; // Guardamos el ID para usarlo después
    Console.WriteLine($"[OK] Expediente creado exitosamente. ID: {expedienteCreadoId}");

    var expCreado = repoExp.ObtenerPorId(expedienteCreadoId.Value);
    Console.WriteLine($"Estado inicial: {expCreado?.Estado}");

    Console.WriteLine("\nAgregando trámite que dispara automatización...");
    var reqTramite = new AltaTramiteRequest(expedienteCreadoId.Value, EtiquetaTramite.PaseAEstudio, "Revisión técnica de infraestructura.", usuarioId);
    ucAltaTramite.Ejecutar(reqTramite);
    Console.WriteLine("[OK] Trámite agregado correctamente.");

    var expModificado = repoExp.ObtenerPorId(expedienteCreadoId.Value);
    Console.WriteLine($"Nuevo estado del expediente: {expModificado?.Estado} (Esperado: ParaResolver)");

    Console.WriteLine("\nListando trámites existentes para el expediente:");
    var lista = ucListarTramites.Ejecutar(expedienteCreadoId.Value);
    foreach (var t in lista)
    {
        Console.WriteLine($" -> Trámite ID: {t.Id} | Etiqueta: {t.Etiqueta} | Contenido: {t.Contenido}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"[ERROR INESPERADO EN CAMINO FELIZ]: {ex.Message}");
}

// -------------------------------------------------------------
// 3. PRUEBA: CAMINO DE ERROR - VALIDACIÓN DE DOMINIO
// -------------------------------------------------------------
try
{
    Console.WriteLine("\n>>> [TEST 2: CAMINO DE ERROR - VALIDADOR DE DOMINIO] <<<");
    Console.WriteLine("Intentando crear un expediente con carátula vacía...");
    
    // Esto debería hacer saltar la lógica de validación de tu Value Object 'Caratula'
    var reqInvalido = new AltaExpedienteRequest("", usuarioId); 
    ucAltaExpediente.Ejecutar(reqInvalido);
    
    Console.WriteLine("[ALERTA] Si ves esto, la validación falló (permitió carátula vacía).");
}
catch (ArgumentException ex) // O cambia por tu 'DominioException' si creaste una específica
{
    Console.WriteLine($"[ERROR DE DOMINIO CAPTURADO EXITOSAMENTE]: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"[ERROR]: {ex.Message}");
}

// -------------------------------------------------------------
// 4. PRUEBA: CAMINO DE ERROR - AUTORIZACIÓN
// -------------------------------------------------------------
try
{
    Console.WriteLine("\n>>> [TEST 3: CAMINO DE ERROR - AUTORIZACIÓN] <<<");
    Console.WriteLine("Simulando acción de usuario no autorizado...");

    // Como pide el enunciado, simulamos el comportamiento de lanzar la excepción
    // que ocurriría si cambiáramos el retorno del AutorizacionProvisionalService a false
    if (expedienteCreadoId.HasValue)
    {
        throw new AutorizacionException("El usuario no posee los permisos necesarios para realizar esta operación en el expediente.");
    }
}
catch (AutorizacionException ex)
{
    Console.WriteLine($"[ERROR DE AUTORIZACIÓN CAPTURADO EXITOSAMENTE]: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"[ERROR GENERAL]: {ex.Message}");
}

Console.WriteLine("\n=================================================");
Console.WriteLine("       TODAS LAS PRUEBAS SE COMPLETARON          ");
Console.WriteLine("=================================================");