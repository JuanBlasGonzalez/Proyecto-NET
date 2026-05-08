namespace SGE.Aplicacion.Expedientes;
// Modificación Carátula
public record ModificarCaratulaRequest(Guid IdExpediente, string NuevaCaratula, Guid UsuarioId);