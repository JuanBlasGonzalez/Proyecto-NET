namespace SGE.Aplicacion.Interfaces;

public interface IPasswordHasher
{
    string CalcularHash(string contrasenaEnTextoPlano);
    bool VerificarHash(string contrasenaEnTextoPlano, string hashAlmacenado);
}