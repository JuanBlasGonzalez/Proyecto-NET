namespace SGE.Aplicacion.Expedientes;

// Este record representa un Data Transfer Object (DTO) para un expediente.
public record ExpedienteDTO(Guid Id, string Caratula, string Estado, DateTime FechaUltimaModificacion);