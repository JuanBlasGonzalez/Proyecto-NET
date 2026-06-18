using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Aplicacion.Expedientes; 
using SGE.Dominio.Expedientes;   
using SGE.Infraestructura.Persistencia.Sqlite.Models;

namespace SGE.Infraestructura.Persistencia.Sqlite;

public class ExpedientesSqliteRepository : IExpedienteRepository
{
    private readonly SgeContext _context;

    // Se recibe el contexto único por Inyección de Dependencias
    public ExpedientesSqliteRepository(SgeContext context)
    {
        _context = context;
    }

    public void Agregar(Expediente expediente)
    {
        var model = new ExpedienteModel
        {
            Id = expediente.Id,
            Caratula = expediente.Caratula.Valor,
            Estado = expediente.Estado.ToString(),
            FechaCreacion = expediente.FechaCreacion,
            FechaUltimaModificacion = expediente.FechaUltimaModificacion,
            UsuarioUltimoCambio = expediente.UsuarioUltimoCambio
        };
        
        _context.Expedientes.Add(model); // Solo marcamos en memoria
    }

    public Expediente? ObtenerPorId(Guid id)
    {
        var model = _context.Expedientes.FirstOrDefault(e => e.Id == id);
        if (model == null) return null;

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
        return _context.Expedientes
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
        var model = _context.Expedientes.FirstOrDefault(e => e.Id == expediente.Id);
        if (model != null)
        {
            model.Caratula = expediente.Caratula.Valor;
            model.Estado = expediente.Estado.ToString();
            model.FechaUltimaModificacion = expediente.FechaUltimaModificacion;
            model.UsuarioUltimoCambio = expediente.UsuarioUltimoCambio;
            
            _context.Expedientes.Update(model); // Indicamos la mutación en memoria
        }
    }

    public void Eliminar(Guid id)
    {
        var model = _context.Expedientes.FirstOrDefault(e => e.Id == id);
        if (model != null)
        {
            _context.Expedientes.Remove(model); // Indicamos la baja en memoria
        }
    }
}