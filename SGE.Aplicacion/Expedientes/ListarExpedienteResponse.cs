namespace SGE.Aplicacion.Expedientes;

// Response formal que encapsula la colección de DTOs
public record ListarExpedientesResponse(IEnumerable<ExpedienteDTO> Expedientes);