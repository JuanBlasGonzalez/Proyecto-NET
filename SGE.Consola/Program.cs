using System;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.ExceptionApp; 
using SGE.Infraestructura.Persistencia;
using SGE.Infraestructura.Servicios;
using SGE.Dominio.Tramites;
using SGE.Dominio.Comun; 
using SGE.Aplicacion.Fecha;

Console.WriteLine("=================================================");
Console.WriteLine("   SGE - SISTEMA DE GESTIÓN DE EXPEDIENTES       ");
Console.WriteLine("       COMPOSITION ROOT & PRUEBAS DE CAPA        ");
Console.WriteLine("=================================================");

// -------------------------------------------------------------
// 1. INICIALIZACIÓN E INYECCIÓN DE DEPENDENCIAS (Composition Root)
// -------------------------------------------------------------
IExpedienteRepository repoExp = new ExpedienteTxtRepository();
ITramiteRepository repoTram = new TramiteTxtRepository();

// CORRECCIÓN: Guardamos la instancia concreta en 'authService' para modificar su propiedad en las pruebas,
// pero mantenemos la inyección a través de la interfaz 'auth' para los Casos de Uso.
var authService = new AutorizacionProvisionalService();
IAutorizacionService auth = authService;

IDateTimeProvider dateTimeProvider = new MachineDateTimeProvider(); 

var servicioEstado = new ActualizacionEstadoExpedienteService(repoExp, repoTram);

var ucAltaExpediente = new AltaExpedienteUseCase(repoExp, auth, dateTimeProvider);
var ucAltaTramite = new AltaTramiteUseCase(repoTram, auth, servicioEstado, dateTimeProvider);
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
    expedienteCreadoId = resExp.Id; 
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
    
    // 🛠️ CORRECCIÓN: Adaptado a la nueva estructura corporativa simétrica de Request y Response
    var reqListar = new ListarTramitesRequest(expedienteCreadoId.Value);
    var responseListar = ucListarTramites.Ejecutar(reqListar);
    
    foreach (var t in responseListar.Tramites)
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
    
    var reqInvalido = new AltaExpedienteRequest("", usuarioId); 
    ucAltaExpediente.Ejecutar(reqInvalido);
    
    Console.WriteLine("[ALERTA] Si ves esto, la validación falló (permitió carátula vacía).");
}
catch (DominioException ex) // Cambiado a DominioException (la que tiran tus Value Objects al fallar)
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

    //CORRECCIÓN OBSERVACIÓN: En vez de un throw manual e inventado acá, modificamos el booleano del método
    // que consulta por la autorización. Esto producirá la excepción legítimamente adentro del Caso de Uso.
    authService.SimularSinPermisos = true;

    if (expedienteCreadoId.HasValue)
    {
        var reqTramiteInvalido = new AltaTramiteRequest(expedienteCreadoId.Value, EtiquetaTramite.PaseAEstudio, "Trámite de prueba sin permisos.", usuarioId);
        ucAltaTramite.Ejecutar(reqTramiteInvalido);
    }
}
catch (AutorizacionException ex)
{
    // Aquí SOLO capturamos la excepción orgánica generada por el Caso de Uso
    Console.WriteLine($"[ERROR DE AUTORIZACIÓN CAPTURADO EXITOSAMENTE]: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"[ERROR GENERAL]: {ex.Message}");
}
finally
{
    //Desactivamos la simulación de falta de permisos para no afectar otras pruebas o usos posteriores.
    authService.SimularSinPermisos = false;
}

Console.WriteLine("\n=================================================");
Console.WriteLine("       TODAS LAS PRUEBAS SE COMPLETARON          ");
Console.WriteLine("=================================================");