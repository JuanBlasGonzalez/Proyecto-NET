using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Aplicacion.Tramites; 
using SGE.Dominio.Tramites;   
using SGE.Infraestructura.Persistencia.Sqlite.Models;

namespace SGE.Infraestructura.Persistencia.Sqlite;

public class TramitesSqliteRepository : ITramiteRepository
{
    public void Agregar(Tramite tramite)
    {
        using var context = new SgeContext();
        
        var model = new TramiteModel
        {
            Id = tramite.Id,
            ExpedienteId = tramite.ExpedienteId,
            Etiqueta = tramite.Etiqueta.ToString(),
            Contenido = tramite.Contenido.Valor, // Value Object a string
            FechaCreacion = tramite.FechaCreacion,
            FechaUltimaModificacion = tramite.FechaUltimaModificacion,
            UsuarioUltimoCambio = tramite.UsuarioUltimoCambio
        };
        
        context.Tramites.Add(model);
        context.SaveChanges();
    }

    public Tramite? ObtenerPorId(Guid id)
    {
        using var context = new SgeContext();
        var model = context.Tramites.FirstOrDefault(t => t.Id == id);
        
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
        using var context = new SgeContext();
        
        return context.Tramites
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
        using var context = new SgeContext();
        var model = context.Tramites.FirstOrDefault(t => t.Id == tramite.Id);
        
        if (model != null)
        {
            model.Etiqueta = tramite.Etiqueta.ToString();
            model.Contenido = tramite.Contenido.Valor;
            model.FechaUltimaModificacion = tramite.FechaUltimaModificacion;
            model.UsuarioUltimoCambio = tramite.UsuarioUltimoCambio;
            
            context.SaveChanges();
        }
    }

    public void Eliminar(Guid id)
    {
        using var context = new SgeContext();
        var model = context.Tramites.FirstOrDefault(t => t.Id == id);
        
        if (model != null)
        {
            context.Tramites.Remove(model);
            context.SaveChanges();
        }
    }
}