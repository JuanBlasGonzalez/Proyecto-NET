using SGE.Dominio.Usuarios;
public record ModificarPermisosRequest(Guid UsuarioObjetivoId, Permiso Permiso, bool Asignar);