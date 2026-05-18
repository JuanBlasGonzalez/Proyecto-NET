namespace SGE.Aplicacion.Expedientes;

public record ModificarCaratulaRequest(Guid IdExpediente, string NuevaCaratula, Guid UsuarioId);