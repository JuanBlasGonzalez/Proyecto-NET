using System;
using System.Collections.Generic;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public record UsuarioDTO(Guid Id, string Nombre, string CorreoElectronico, bool EsAdministrador, List<Permiso> Permisos);