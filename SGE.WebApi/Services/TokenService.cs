using Microsoft.IdentityModel.Tokens;
using SGE.Aplicacion.Interfaces; 
using SGE.Dominio.Usuarios;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SGE.WebApi.Services;

public class TokenService : ITokenService
{
    // Clave secreta para firmar los tokens (mínimo 32 caracteres)
    public static readonly string SecretKey = "SuperSecretKeySGE2026_CatedraNetParallelComputing";

    public string GenerarToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(SecretKey);

        // Cargamos los Claims del usuario que viajaránencriptados en el JWT
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                new Claim("EsAdministrador", usuario.EsAdministrador.ToString().ToLower())
            }),
            Expires = DateTime.UtcNow.AddHours(2), // Vence en 2 horas
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}