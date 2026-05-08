namespace SGE.Aplicacion.Expedientes;

public record ExpedienteDTO(Guid Id, string Caratula, string Estado, DateTime FechaUltimaModificacion);