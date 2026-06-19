using Microsoft.AspNetCore.Mvc;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Expedientes; 
using SGE.Aplicacion.Tramites;    
using SGE.Aplicacion.Autorizacion; 
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.WebApi.Services;
using System.Security.Claims;

namespace SGE.WebApi.Endpoints;

public static class SgeEndpoints
{
    public static void MapSgeEndpoints(this IEndpointRouteBuilder app)
    {
        // ==========================================
        // GRUPO: AUTENTICACIÓN (LOGIN)
        // ==========================================
        app.MapPost("/api/auth/login", (
            [FromBody] LoginApiInput request,
            IUsuarioRepository repoUsuario,
            IPasswordHasher hasher,
            ITokenService tokenService) =>
        {
            var usuario = repoUsuario.ObtenerPorCorreo(request.CorreoElectronico);
            if (usuario == null || !hasher.VerificarHash(request.Contrasena, usuario.ContrasenaHash))
            {
                return Results.Problem(detail: "Credenciales inválidas.", statusCode: 401, title: "No Autorizado");
            }

            var token = tokenService.GenerarToken(usuario);
            return Results.Ok(new { Token = token, Usuario = usuario.Nombre });
        })
        .WithName("Login")
        .WithTags("Autenticación");


        // ==========================================
        // GRUPO: EXPEDIENTES (RUTAS PROTEGIDAS)
        // ==========================================
        var expedientesGrupo = app.MapGroup("/api/expedientes").RequireAuthorization().WithTags("Expedientes");

        // Alta de Expediente
        expedientesGrupo.MapPost("/", (
            [FromBody] CrearExpedienteApiInput apiRequest,
            AltaExpedienteUseCase usoDeCaso,
            ClaimsPrincipal user) =>
        {
            var usuarioId = ObtenerUsuarioId(user);
            
            // Empaquetamos en tu Request de Aplicación real
            var appRequest = new AltaExpedienteRequest(apiRequest.Caratula, usuarioId);
            var response = usoDeCaso.Ejecutar(appRequest);
            
            return Results.Created($"/api/expedientes/{response.Id}", response);
        });

        // Consulta de Todos los Expedientes
        expedientesGrupo.MapGet("/", (ListarExpedientesUseCase usoDeCaso) =>
        {
            var response = usoDeCaso.Ejecutar(new ListarExpedientesRequest()); // Devuelve tu ListarExpedientesResponse
            return Results.Ok(response);
        });

        // Modificación de Expediente
        expedientesGrupo.MapPut("/{id:guid}", (
            Guid id, 
            [FromBody] ModificarExpedienteApiInput apiRequest, 
            ModificarCaratulaExpedienteUseCase usoCaratula,
            CambiarEstadoExpedienteUseCase usoEstado,
            ClaimsPrincipal user) =>
        {
            var usuarioId = ObtenerUsuarioId(user);
            
            // Modificamos carátula si viene informada
            var requestCaratula = new ModificarCaratulaRequest(id, apiRequest.Caratula, usuarioId);
            usoCaratula.Ejecutar(requestCaratula);

            // Cambiamos el estado si viene informado
            var requestEstado = new CambiarEstadoRequest(id, apiRequest.Estado, usuarioId);
            usoEstado.Ejecutar(requestEstado);
            
            return Results.NoContent();
        });

        // Baja de Expediente
        expedientesGrupo.MapDelete("/{id:guid}", (
            Guid id, 
            BajaExpedienteUseCase usoDeCaso,
            ClaimsPrincipal user) =>
        {
            var usuarioId = ObtenerUsuarioId(user);
            
            var appRequest = new BajaExpedienteRequest(id, usuarioId);
            usoDeCaso.Ejecutar(appRequest);
            
            return Results.NoContent();
        });


        // ==========================================
        // GRUPO: TRÁMITES (RUTAS PROTEGIDAS)
        // ==========================================
        var tramitesGrupo = app.MapGroup("/api/tramites").RequireAuthorization().WithTags("Trámites");

        // Alta de Trámite
        tramitesGrupo.MapPost("/", (
            [FromBody] CrearTramiteApiInput apiRequest,
            AltaTramiteUseCase usoDeCaso,
            ClaimsPrincipal user) =>
        {
            var usuarioId = ObtenerUsuarioId(user);
            
            // Se instancia el Request con los 4 parámetros agrupados
            var appRequest = new AltaTramiteRequest(
                apiRequest.ExpedienteId, 
                apiRequest.Etiqueta, 
                apiRequest.Contenido, 
                usuarioId
            );
            
            var response = usoDeCaso.Ejecutar(appRequest);
            return Results.Created(string.Empty, response);
        });

        // Consulta de Trámites por ID de Expediente
        tramitesGrupo.MapGet("/expediente/{expedienteId:guid}", (Guid expedienteId, ListarTramitesPorExpedienteUseCase usoDeCaso) =>
        {
            var appRequest = new ListarTramitesRequest(expedienteId);
            var response = usoDeCaso.Ejecutar(appRequest);
            return Results.Ok(response);
        });

        // Modificación de Trámite
        tramitesGrupo.MapPut("/{id:guid}", (
            Guid id, 
            [FromBody] ModificarTramiteApiInput apiRequest, 
            ModificarTramiteUseCase usoDeCaso,
            ClaimsPrincipal user) =>
        {
            var usuarioId = ObtenerUsuarioId(user);
            
            var appRequest = new ModificarTramiteRequest(id, apiRequest.Contenido, usuarioId);
            usoDeCaso.Ejecutar(appRequest);
            
            return Results.NoContent();
        });

        // Baja de Trámite
        tramitesGrupo.MapDelete("/{id:guid}", (
            Guid id, 
            BajaTramiteUseCase usoDeCaso,
            ClaimsPrincipal user) =>
        {
            var usuarioId = ObtenerUsuarioId(user);
            
            var appRequest = new BajaTramiteRequest(id, usuarioId);
            usoDeCaso.Ejecutar(appRequest);
            
            return Results.NoContent();
        });
    }

    private static Guid ObtenerUsuarioId(ClaimsPrincipal user)
    {
        var claimId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId))
            throw new AutorizacionException("Token inválido: Identidad ausente.");
        
        return Guid.Parse(claimId);
    }
}

// DTOs de entrada limpios mapeados desde el JSON entrante de HTTP (Evitamos colisiones de nombres)
public record LoginApiInput(string CorreoElectronico, string Contrasena);
public record CrearExpedienteApiInput(string Caratula);
public record ModificarExpedienteApiInput(string Caratula, EstadoExpediente Estado);
public record CrearTramiteApiInput(Guid ExpedienteId, EtiquetaTramite Etiqueta, string Contenido);
public record ModificarTramiteApiInput(EtiquetaTramite Etiqueta, string Contenido);