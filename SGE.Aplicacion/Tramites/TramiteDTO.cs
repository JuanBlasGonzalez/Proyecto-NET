namespace SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;

// Esta clase es un DTO (Data Transfer Object) que se utiliza para transferir datos relacionados con un trámite entre diferentes capas de la aplicación.
// MODIFICACION: En esta seccion se corrigio la declaracion a tipo "record" y se agregaron todos los atributos faltantes.
public record TramiteDTO(
    Guid Id,
    Guid ExpedienteId,
    EtiquetaTramite Etiqueta,
    string Contenido,
    DateTime FechaCreacion,
    DateTime FechaUltimaModificacion,
    Guid UsuarioUltimoCambio
);