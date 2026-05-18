using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

// Esta interfaz define el contrato para un repositorio de trámites.
public interface ITramiteRepository
{
    void Agregar(Tramite tramite);
    void Eliminar(Guid id);
    void Modificar(Tramite tramite);
    Tramite? ObtenerPorId(Guid id);
    IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId);
}