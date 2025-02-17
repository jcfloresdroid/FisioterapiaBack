using System.Security.Claims;
using Core.Services.Interfaz;

namespace Presentation.Services;

public class Authorization : IAuthorization
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public Authorization(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public int UsuarioActual()
    {
        ClaimsIdentity Identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
        int usuarioActual = int.Parse(Identity.FindFirst(ClaimTypes.Name).Value);
        return usuarioActual;
    }
}