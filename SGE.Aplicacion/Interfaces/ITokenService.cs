using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Interfaces;

public interface ITokenService
{
    string GenerarToken(Usuario usuario);
}