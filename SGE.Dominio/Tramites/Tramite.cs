using System;

namespace SGE.Dominio.Tramites;

// Clase que representa un tramite.
public class Tramite
{
    public Guid Id { get; private set; }
    public Guid ExpedienteId { get; private set; } // La "llave" para saber de qué expediente es
    public EtiquetaTramite Etiqueta { get; private set; }
    public ContenidoTramite Contenido { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimaModificacion { get; private set; }
    public Guid UsuarioUltimoCambio { get; private set; }

    private Tramite() 
    {
        // Constructor vacío para uso exclusivo de Reconstruir
    }

    // Constructor para el "Alta" de un tramite
    public Tramite(Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid usuarioId,DateTime fechaCreacion)
    {
        Id = Guid.NewGuid();
        ExpedienteId = expedienteId;
        Etiqueta = etiqueta;
        Contenido = contenido;
        UsuarioUltimoCambio = usuarioId;
        FechaCreacion = fechaCreacion;
        FechaUltimaModificacion = fechaCreacion;
    }

    // Permite modificar el contenido del trámite, y al mismo tiempo actualiza el usuario que hizo el cambio y la fecha de modificación.
    public void ModificarContenido(ContenidoTramite nuevoContenido, Guid usuarioId,DateTime fechaModificacion)
    {
        this.Contenido = nuevoContenido;
        this.FechaUltimaModificacion = fechaModificacion;
        this.UsuarioUltimoCambio = usuarioId;
    }

    // Este método estático se utiliza para reconstruir un trámite a partir de sus propiedades, lo que es útil para la persistencia y recuperación de datos.
    public static Tramite Reconstruir(Guid id, Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, DateTime fechaCreacion, DateTime fechaUltimaModificacion, Guid usuarioUltimoCambio)
    {
        var tramite = new Tramite();
        tramite.Id = id;
        tramite.ExpedienteId = expedienteId;
        tramite.Etiqueta = etiqueta;
        tramite.Contenido = contenido;
        tramite.FechaCreacion = fechaCreacion;
        tramite.FechaUltimaModificacion = fechaUltimaModificacion;
        tramite.UsuarioUltimoCambio = usuarioUltimoCambio;
        
        return tramite;
    }
}
