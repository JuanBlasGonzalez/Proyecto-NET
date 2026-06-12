namespace SGE.Aplicacion.Expedientes;
using SGE.Dominio.Expedientes;

// Este record representa un Data Transfer Object (DTO) para un expediente.
//Actualizacion: Agregamos los campos faltantes solicitados
public record ExpedienteDTO(
    Guid Id, 
    string Caratula, 
    DateTime FechaCreacion, 
    DateTime FechaUltimaModificacion,
    Guid UsuarioUltimoCambio,
    EstadoExpediente Estado);