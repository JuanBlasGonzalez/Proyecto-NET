using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SGE.Aplicacion.ExceptionApp;    // Para EntidadNoEncontradaException
using SGE.Aplicacion.Autorizacion;    // Para AutorizacionException
using SGE.Dominio.Comun;              // Para DominioException
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SGE.WebApi.Middlewares;

public class ManejadorExcepciones : IExceptionHandler
{
    private readonly ILogger<ManejadorExcepciones> _logger;

    public ManejadorExcepciones(ILogger<ManejadorExcepciones> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Ocurrió un error en el sistema: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        switch (exception)
        {
            // Retorna 403 Forbidden
            case AutorizacionException:
                problemDetails.Status = (int)HttpStatusCode.Forbidden; 
                problemDetails.Title = "Acceso Denegado";
                problemDetails.Detail = exception.Message;
                break;

            // Retorna 404 Not Found
            case EntidadNoEncontradaException:
                problemDetails.Status = (int)HttpStatusCode.NotFound; 
                problemDetails.Title = "Recurso No Encontrado";
                problemDetails.Detail = exception.Message;
                break;

            // Errores de validación de Dominio retornan 400 Bad Request
            case DominioException:
            case ArgumentException:
            case InvalidOperationException:
                problemDetails.Status = (int)HttpStatusCode.BadRequest; 
                problemDetails.Title = "Solicitud Incorrecta (Error de Validación)";
                problemDetails.Detail = exception.Message;
                break;

            default:
                problemDetails.Status = (int)HttpStatusCode.InternalServerError; // 500
                problemDetails.Title = "Error Interno del Servidor";
                problemDetails.Detail = "Ocurrió un error inesperado en el sistema.";
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true; 
    }
}