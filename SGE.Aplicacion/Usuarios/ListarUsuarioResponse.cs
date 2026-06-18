using System;
using System.Collections.Generic;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public record ListarUsuariosResponse(List<UsuarioDTO> Usuarios);