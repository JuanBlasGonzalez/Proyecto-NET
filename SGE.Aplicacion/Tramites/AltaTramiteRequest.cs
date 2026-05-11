namespace SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;

public record AltaTramiteRequest(
    Guid ExpedienteId, 
    EtiquetaTramite Etiqueta, 
    string Contenido, 
    Guid UsuarioId
);