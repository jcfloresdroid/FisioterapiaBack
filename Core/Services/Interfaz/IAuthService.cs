using Core.Services.Implementacion;

namespace Core.Services.Interfaz;

public interface IAuthService
{
    Task<JWTSettings> AuthenticateAsync(string email, string password, bool rememberMe);
}