using System;
using System.Collections.Generic;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Interfaces;

public interface IUsuarioRepository
{
    void Agregar(Usuario usuario);
    void Modificar(Usuario usuario); // La agregamos para cuando se cambien datos o permisos
    void Eliminar(Guid id);
    Usuario? ObtenerPorId(Guid id);
    Usuario? ObtenerPorCorreo(string correo);
    List<Usuario> ObtenerTodos();
}