using System;

namespace SGE.Dominio.Tramites;

public class Tramite
{
    public Guid Id { get; private set; }
    public Guid ExpedienteId { get; private set; } // La "llave" para saber de qué expediente es
    public EtiquetaTramite Etiqueta { get; private set; }
    public ContenidoTramite Contenido { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimaModificacion { get; private set; }
    public Guid UsuarioUltimoCambio { get; private set; }

    public Tramite(Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid usuarioId)
    {
        Id = Guid.NewGuid();
        ExpedienteId = expedienteId;
        Etiqueta = etiqueta;
        Contenido = contenido;
        UsuarioUltimoCambio = usuarioId;
        FechaCreacion = DateTime.Now;
        FechaUltimaModificacion = FechaCreacion;
    }

    public void ModificarContenido(ContenidoTramite nuevoContenido, Guid usuarioId)
    {
        this.Contenido = nuevoContenido;
        this.FechaUltimaModificacion = DateTime.Now;
        this.UsuarioUltimoCambio = usuarioId;
    }
}
