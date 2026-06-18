using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Aplicacion.Tramites; 
using SGE.Dominio.Tramites;   
using SGE.Infraestructura.Persistencia.Sqlite.Models;

namespace SGE.Infraestructura.Persistencia.Sqlite;

public class TramitesSqliteRepository : ITramiteRepository
{
    private readonly SgeContext _context;

    public TramitesSqliteRepository(SgeContext context)
    {
        _context = context;
    }

    public void Agregar(Tramite tramite)
    {
        var model = new TramiteModel
        {
            Id = tramite.Id,
            ExpedienteId = tramite.ExpedienteId,
            Etiqueta = tramite.Etiqueta.ToString(),
            Contenido = tramite.Contenido.Valor,
            FechaCreacion = tramite.FechaCreacion,
            FechaUltimaModificacion = tramite.FechaUltimaModificacion,
            UsuarioUltimoCambio = tramite.UsuarioUltimoCambio
        };
        
        _context.Tramites.Add(model);
    }

    public Tramite? ObtenerPorId(Guid id)
    {
        var model = _context.Tramites.FirstOrDefault(t => t.Id == id);
        if (model == null) return null;

        return Tramite.Reconstruir(
            model.Id,
            model.ExpedienteId,
            Enum.Parse<EtiquetaTramite>(model.Etiqueta),
            new ContenidoTramite(model.Contenido),
            model.FechaCreacion,
            model.FechaUltimaModificacion,
            model.UsuarioUltimoCambio
        );
    }

    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId)
    {
        return _context.Tramites
            .Where(t => t.ExpedienteId == expedienteId)
            .AsEnumerable()
            .Select(model => Tramite.Reconstruir(
                model.Id,
                model.ExpedienteId,
                Enum.Parse<EtiquetaTramite>(model.Etiqueta),
                new ContenidoTramite(model.Contenido),
                model.FechaCreacion,
                model.FechaUltimaModificacion,
                model.UsuarioUltimoCambio
            ))
            .ToList();
    }

    public void Modificar(Tramite tramite)
    {
        var model = _context.Tramites.FirstOrDefault(t => t.Id == tramite.Id);
        if (model != null)
        {
            model.Etiqueta = tramite.Etiqueta.ToString();
            model.Contenido = tramite.Contenido.Valor;
            model.FechaUltimaModificacion = tramite.FechaUltimaModificacion;
            model.UsuarioUltimoCambio = tramite.UsuarioUltimoCambio;
            
            _context.Tramites.Update(model);
        }
    }

    public void Eliminar(Guid id)
    {
        var model = _context.Tramites.FirstOrDefault(t => t.Id == id);
        if (model != null)
        {
            _context.Tramites.Remove(model);
        }
    }
}