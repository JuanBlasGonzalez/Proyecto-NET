using SGE.Aplicacion.Interfaces;
using System;

namespace SGE.Aplicacion.Usuarios;

public class LoginUseCase(IUsuarioRepository repo, IPasswordHasher hasher, ITokenService tokenService)
{
    public LoginResponse Ejecutar(LoginRequest request)
    {
        // Buscar por correo como identificador único
        var usuario = repo.ObtenerPorCorreo(request.CorreoElectronico);
        if (usuario == null)
            throw new Exception("Credenciales incorrectas.");

        // Recalcular el hash y comparar identidades (Requisito de Diseño 2.1)
        if (!hasher.VerificarHash(request.Contrasena, usuario.ContrasenaHash))
            throw new Exception("Credenciales incorrectas.");

        // Retorna el token JWT que transportará el UserId de forma segura en las peticiones HTTP
        string token = tokenService.GenerarToken(usuario);
        return new LoginResponse(token, true);
    }
}