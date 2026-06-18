using System;
using System.Linq;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.ExceptionApp;

namespace SGE.Aplicacion.Usuarios;

public class ListarUsuariosUseCase(IUsuarioRepository repo)
{
    public ListarUsuariosResponse Ejecutar(Guid ejecutorId, ListarUsuariosRequest request)
    {
        // REGLA DE CONTROL OBLIGATORIA: Primera instancia de verificación
        var ejecutor = repo.ObtenerPorId(ejecutorId);
        if (ejecutor == null || !ejecutor.EsAdministrador)
            throw new AutorizacionException("Acción denegada: Se requieren privilegios de Administrador.");

        // Obtener todas las entidades del repositorio
        var usuarios = repo.ObtenerTodos();

        // Mapear las entidades de Dominio a los DTOs de lectura seguros del Response
        var dtos = usuarios.Select(u => new UsuarioDTO(
            u.Id,
            u.Nombre,
            u.CorreoElectronico,
            u.EsAdministrador,
            u.Permisos.ToList()
        )).ToList();

        return new ListarUsuariosResponse(dtos);
    }
}