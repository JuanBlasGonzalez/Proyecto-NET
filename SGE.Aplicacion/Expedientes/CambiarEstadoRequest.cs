using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public record CambiarEstadoRequest(Guid IdExpediente, EstadoExpediente NuevoEstado, Guid UsuarioId);