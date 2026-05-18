namespace SGE.Dominio.Expedientes;

//Enum que representa los diferentes estados que puede tener un expediente a lo largo de su ciclo de vida.
public enum EstadoExpediente
{
    RecienIniciado, 
    ParaResolver, 
    ConResolucion, 
    EnNotificacion, 
    Finalizado
}
