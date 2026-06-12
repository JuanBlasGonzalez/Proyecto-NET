using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Aplicacion.Expedientes; 
using SGE.Dominio.Expedientes;   
using SGE.Infraestructura.Persistencia.Sqlite.Models;

namespace SGE.Infraestructura.Persistencia.Sqlite;

public class ExpedientesSqliteRepository : IExpedienteRepository
{
    public void Agregar(Expediente expediente)
    {
        using var context = new SgeContext();
        
        var model = new ExpedienteModel
        {
            Id = expediente.Id,
            Caratula = expediente.Caratula.Valor, // Value Object a string
            Estado = expediente.Estado.ToString(), // Enum a string
            FechaCreacion = expediente.FechaCreacion,
            FechaUltimaModificacion = expediente.FechaUltimaModificacion,
            UsuarioUltimoCambio = expediente.UsuarioUltimoCambio
        };
        
        context.Expedientes.Add(model);
        context.SaveChanges();
    }

    public Expediente? ObtenerPorId(Guid id)
    {
        using var context = new SgeContext();
        var model = context.Expedientes.FirstOrDefault(e => e.Id == id);
        
        if (model == null) return null;

        // Usamos el Factory Method 'Reconstruir' en vez del constructor
        return Expediente.Reconstruir(
            model.Id,
            new Caratula(model.Caratula),
            model.FechaCreacion,
            model.FechaUltimaModificacion,
            model.UsuarioUltimoCambio,
            Enum.Parse<EstadoExpediente>(model.Estado)
        );
    }

    public IEnumerable<Expediente> ObtenerTodos()
    {
        using var context = new SgeContext();
        
        return context.Expedientes
            .AsEnumerable() 
            .Select(model => Expediente.Reconstruir(
                model.Id,
                new Caratula(model.Caratula),
                model.FechaCreacion,
                model.FechaUltimaModificacion,
                model.UsuarioUltimoCambio,
                Enum.Parse<EstadoExpediente>(model.Estado)
            ))
            .ToList();
    }

    public void Modificar(Expediente expediente)
    {
        using var context = new SgeContext();
        var model = context.Expedientes.FirstOrDefault(e => e.Id == expediente.Id);
        
        if (model != null)
        {
            model.Caratula = expediente.Caratula.Valor;
            model.Estado = expediente.Estado.ToString();
            model.FechaUltimaModificacion = expediente.FechaUltimaModificacion;
            model.UsuarioUltimoCambio = expediente.UsuarioUltimoCambio;
            
            context.SaveChanges();
        }
    }

    public void Eliminar(Guid id)
    {
        using var context = new SgeContext();
        var model = context.Expedientes.FirstOrDefault(e => e.Id == id);
        
        if (model != null)
        {
            context.Expedientes.Remove(model);
            context.SaveChanges();
        }
    }
}