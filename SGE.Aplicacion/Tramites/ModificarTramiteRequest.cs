namespace SGE.Aplicacion.Tramites;
public record ModificarTramiteRequest(Guid IdTramite, string NuevoContenido, Guid UsuarioId);