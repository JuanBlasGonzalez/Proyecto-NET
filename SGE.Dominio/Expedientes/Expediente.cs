namespace SGE.Dominio.Expedientes;

using System;
using SGE.Dominio.Tramites; 
using SGE.Dominio.Comun;

public class Expediente
{
    //Propiedades: Acceso publico para lectura (get), pero solo la clase puede modificar el atributo por el private
    public Guid Id { get; private set; } 
    public Caratula Caratula { get; private set; } //Value object q valida el texto
    public DateTime FechaCreacion { get; private set; } 
    public DateTime FechaUltimaModificacion { get; private set; } 
    public Guid UsuarioUltimoCambio { get; private set; } 
    public EstadoExpediente Estado { get; private set; }

    // Constructor para el "Alta" de un expediente 
    public Expediente(Caratula caratula, Guid idUsuario)
    {
        Id = Guid.NewGuid(); //Generacion automatica del ID
        Caratula = caratula; 
        UsuarioUltimoCambio = idUsuario; 
        //Al crear el expediente ambas fechas son iguales 
        FechaCreacion = DateTime.Now; 
        FechaUltimaModificacion = FechaCreacion; 
        Estado = EstadoExpediente.RecienIniciado; //Al crear el exp siempre es RecienIniciado
    }

    public void ModificarCaratula (Caratula nuevaCaratula, Guid idUsuario)
    {
        this.Caratula = nuevaCaratula;
        this.UsuarioUltimoCambio = idUsuario;
        this.FechaUltimaModificacion = DateTime.Now;
    }

    public bool ActualizarEstado (EtiquetaTramite? ultimaEtiqueta, Guid idUsuario)
    {
        // Guardamos el estado anterior para saber si realmente hubo un cambio al final
        EstadoExpediente estadoAnterior = this.Estado;

        // Aplicamos las reglas de negocio del PDF
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
            this.FechaUltimaModificacion = DateTime.Now;
            return true; // Hubo cambio
        }

        return false; // No hubo cambio
    }

    public void CambiarEstado (EstadoExpediente nuevoEstado, Guid idUsuario)
    {
        // Simplemente asignamos el nuevo estado enviado
        this.Estado = nuevoEstado;
        this.UsuarioUltimoCambio = idUsuario;
        this.FechaUltimaModificacion = DateTime.Now;
    }
}
