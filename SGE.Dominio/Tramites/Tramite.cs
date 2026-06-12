using System;

namespace SGE.Dominio.Tramites;
using SGE.Dominio.Comun;

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
        // Constructor vacío para la persistencia de datos.
    }

    public Tramite(Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid usuarioId, DateTime fechaCreacion)
        : this(Guid.NewGuid(), expedienteId, etiqueta, contenido, fechaCreacion, fechaCreacion, usuarioId)
    {
        // Delega todo al constructor privado centralizado
    }

    //Constructur privado para la asignacion y validacion del objeto
    private Tramite(Guid id, Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, DateTime fechaCreacion, DateTime fechaUltimaModificacion, Guid usuarioUltimoCambio)
    {
        // VALIDACIONES SOLICITADAS EN LAS OBSERVACIONES
        if (id == Guid.Empty) 
            throw new DominioException("El ID del trámite no puede ser vacío.");
        
        if (expedienteId == Guid.Empty) 
            throw new DominioException("El ID de expediente asociado no puede ser vacío.");
        
        if (usuarioUltimoCambio == Guid.Empty) 
            throw new DominioException("El usuario del último cambio no puede ser vacío.");

        if (fechaUltimaModificacion < fechaCreacion) 
            throw new DominioException("La fecha de última modificación no puede ser anterior a la de creación.");

        if (fechaCreacion > DateTime.Now)
            throw new DominioException("La fecha de creación no puede estar en el futuro.");

        //Nuevamente, luego de la correcta validacion de parametros, realizamos la asignacion.
        Id = id;
        ExpedienteId = expedienteId;
        Etiqueta = etiqueta;
        Contenido = contenido ?? throw new DominioException("El contenido del trámite no puede ser nulo.");
        FechaCreacion = fechaCreacion;
        FechaUltimaModificacion = fechaUltimaModificacion;
        UsuarioUltimoCambio = usuarioUltimoCambio;
    }

    // Permite modificar el contenido del trámite, y al mismo tiempo actualiza el usuario que hizo el cambio y la fecha de modificación.
    public void ModificarContenido(ContenidoTramite nuevoContenido, Guid usuarioId,DateTime fechaModificacion)
    {
        //NUEVO: VALIDACIONES AGREGADAS 
        if (usuarioId == Guid.Empty) throw new DominioException("El usuario no puede ser vacío.");
        if (fechaModificacion < this.FechaCreacion) throw new DominioException("La fecha de modificación es inválida.");

        this.Contenido = nuevoContenido ?? throw new DominioException("El contenido del trámite no puede ser nulo.");
        this.FechaUltimaModificacion = fechaModificacion;
        this.UsuarioUltimoCambio = usuarioId;
    }

    //Este método estático se utiliza para reconstruir un trámite a partir de sus propiedades, lo que es útil para la persistencia y recuperación de datos.
    //FACTORY METHOD.
    public static Tramite Reconstruir(Guid id, Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, DateTime fechaCreacion, DateTime fechaUltimaModificacion, Guid usuarioUltimoCambio)
    {
       return new Tramite(id, expedienteId, etiqueta, contenido, fechaCreacion, fechaUltimaModificacion, usuarioUltimoCambio);
    }
}
