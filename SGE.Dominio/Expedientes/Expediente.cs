namespace SGE.Dominio.Expedientes;

using System;
using SGE.Dominio.Tramites; 
using SGE.Dominio.Comun;
// Esta clase representa un expediente, que es una entidad con identidad propia y un ciclo de vida definido.
public class Expediente
{
    //Propiedades: Acceso publico para lectura (get), pero solo la clase puede modificar el atributo por el private
    public Guid Id { get; private set; } 
    public Caratula Caratula { get; private set; } //Value object q valida el texto
    public DateTime FechaCreacion { get; private set; } 
    public DateTime FechaUltimaModificacion { get; private set; } 
    public Guid UsuarioUltimoCambio { get; private set; } 
    public EstadoExpediente Estado { get; private set; }

    private Expediente() 
    {
        // Constructor vacío para que la persistencia arme el objeto
    }

    // Constructor para el "Alta" de un expediente 
    public Expediente(Caratula caratula, Guid idUsuario,DateTime fechaCreacion)
    {
        Id = Guid.NewGuid(); //Generacion automatica del ID
        Caratula = caratula; 
        UsuarioUltimoCambio = idUsuario; 
        //Al crear el expediente ambas fechas son iguales 
        FechaCreacion = fechaCreacion; 
        FechaUltimaModificacion = fechaCreacion; 
        Estado = EstadoExpediente.RecienIniciado; //Al crear el exp siempre es RecienIniciado
    }

    //Este metodo permite modificar la caratula del expediente, y al mismo tiempo actualiza el usuario que hizo el cambio y la fecha de modificación.
    public void ModificarCaratula (Caratula nuevaCaratula, Guid idUsuario,DateTime fechaModificacion)
    {
        this.Caratula = nuevaCaratula;
        this.UsuarioUltimoCambio = idUsuario;
        this.FechaUltimaModificacion = fechaModificacion;
    }

    //Este método actualiza el estado del expediente según la última etiqueta de trámite aplicada, y también actualiza el usuario que hizo el cambio y la fecha de modificación. 
    //Devuelve un booleano indicando si hubo un cambio de estado o no.
    public bool ActualizarEstado (EtiquetaTramite? ultimaEtiqueta, Guid idUsuario, DateTime fechaModificacion)
    {
        // Guardamos el estado anterior para saber si realmente hubo un cambio al final
        EstadoExpediente estadoAnterior = this.Estado;

        // Aplicamos las reglas de negocio del enunciado
        switch (ultimaEtiqueta)
        {
            case EtiquetaTramite.PaseAEstudio:
                this.Estado = EstadoExpediente.ParaResolver;
                break;
            case EtiquetaTramite.Resolucion:
                this.Estado = EstadoExpediente.ConResolucion;
                break;
            case EtiquetaTramite.Notificacion:
                this.Estado = EstadoExpediente.EnNotificacion;
                break;
            case EtiquetaTramite.PaseAlArchivo:
                this.Estado = EstadoExpediente.Finalizado;
                break;
            default:
                // Si la etiqueta es EscritoPresentado, Despacho o nula, no indica cambio de estado automático.
                break;
        }

        // Si el estado cambió después del switch, actualizamos 
        if (this.Estado != estadoAnterior)
        {
            this.UsuarioUltimoCambio = idUsuario;
            this.FechaUltimaModificacion = fechaModificacion;
            return true; // Hubo cambio
        }

        return false; // No hubo cambio
    }

    //Este metodo permite cambiar el estado del expediente a cualquier otro
    public void CambiarEstado (EstadoExpediente nuevoEstado, Guid idUsuario,DateTime fechaModificacion)
    {
        // Simplemente asignamos el nuevo estado enviado
        this.Estado = nuevoEstado;
        this.UsuarioUltimoCambio = idUsuario;
        this.FechaUltimaModificacion = fechaModificacion;
    }

    //Este método estático se utiliza para reconstruir un expediente a partir de sus propiedades, lo que es útil para la persistencia y recuperación de datos.
    public static Expediente Reconstruir(Guid id, Caratula caratula, DateTime fechaCreacion, DateTime fechaUltimaModificacion, Guid usuarioUltimoCambio, EstadoExpediente estado)
    {
        var expediente = new Expediente();
        expediente.Id = id;
        expediente.Caratula = caratula;
        expediente.FechaCreacion = fechaCreacion;
        expediente.FechaUltimaModificacion = fechaUltimaModificacion;
        expediente.UsuarioUltimoCambio = usuarioUltimoCambio;
        expediente.Estado = estado;
        
        return expediente;
    }
}
