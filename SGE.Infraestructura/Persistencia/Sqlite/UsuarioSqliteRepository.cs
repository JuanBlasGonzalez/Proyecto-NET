using Microsoft.EntityFrameworkCore;
using SGE.Aplicacion.Interfaces;
using SGE.Dominio.Usuarios;
using SGE.Infraestructura.Persistencia.Sqlite.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SGE.Infraestructura.Persistencia.Sqlite;

public class UsuarioSqliteRepository : IUsuarioRepository
{
    private readonly SgeContext _context;

    public UsuarioSqliteRepository(SgeContext context)
    {
        _context = context;
    }

    public void Agregar(Usuario usuario)
    {
        var model = new UsuarioModel
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            CorreoElectronico = usuario.CorreoElectronico,
            ContrasenaHash = usuario.ContrasenaHash,
            EsAdministrador = usuario.EsAdministrador,
            Permisos = usuario.Permisos.Select(p => new UsuarioPermisoModel
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuario.Id,
                Permiso = p.ToString()
            }).ToList()
        };
        _context.Usuarios.Add(model);
    }

    public void Modificar(Usuario usuario)
    {
        var modelExistente = _context.Usuarios
            .Include(u => u.Permisos)
            .FirstOrDefault(u => u.Id == usuario.Id);

        if (modelExistente != null)
        {
            modelExistente.Nombre = usuario.Nombre;
            modelExistente.CorreoElectronico = usuario.CorreoElectronico;
            modelExistente.ContrasenaHash = usuario.ContrasenaHash;
            modelExistente.EsAdministrador = usuario.EsAdministrador;

            // Limpiamos los permisos viejos de la tabla intermedia y cargamos los nuevos
            _context.UsuariosPermisos.RemoveRange(modelExistente.Permisos);
            
            modelExistente.Permisos = usuario.Permisos.Select(p => new UsuarioPermisoModel
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuario.Id,
                Permiso = p.ToString()
            }).ToList();

            _context.Usuarios.Update(modelExistente);
        }
    }

    public void Eliminar(Guid id)
    {
        var model = _context.Usuarios.Find(id);
        if (model != null)
        {
            _context.Usuarios.Remove(model);
        }
    }

    public Usuario? ObtenerPorId(Guid id)
    {
        var model = _context.Usuarios
            .Include(u => u.Permisos)
            .FirstOrDefault(u => u.Id == id);

        return model == null ? null : MapearADominio(model);
    }

    public Usuario? ObtenerPorCorreo(string correo)
    {
        var model = _context.Usuarios
            .Include(u => u.Permisos)
            .FirstOrDefault(u => u.CorreoElectronico.ToLower() == correo.ToLower());

        return model == null ? null : MapearADominio(model);
    }

    public List<Usuario> ObtenerTodos()
    {
        var modelos = _context.Usuarios.Include(u => u.Permisos).ToList();
        return modelos.Select(MapearADominio).ToList();
    }

    // Método privado para transformar el modelo mutable de EF en la Entidad inmutable de Dominio
    private static Usuario MapearADominio(UsuarioModel model)
    {
        var listaPermisosDominio = new List<Permiso>();
        foreach (var pModel in model.Permisos)
        {
            if (Enum.TryParse<Permiso>(pModel.Permiso, out var permisoEnum))
            {
                listaPermisosDominio.Add(permisoEnum);
            }
        }

        return Usuario.Reconstruir(
            model.Id,
            model.Nombre,
            model.CorreoElectronico,
            model.ContrasenaHash,
            model.EsAdministrador,
            listaPermisosDominio
        );
    }
}