using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Domain.Exceptions;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.Services.Implementacion;

public class AuthService : IAuthService
{
    private readonly FisioContext _context;
    private readonly IConfiguration _configuration;
    
    public AuthService(FisioContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    public async Task<JWTSettings> AuthenticateAsync(string email, string password, bool rememberMe)
    {
        //Devuelve al usuario
        var user = await _context.Usuarios
            .FirstOrDefaultAsync(x => x.Username == email && x.Password == password);
        
        //Si no es encontrado el usuario deniega el acceso
        if(user == null)
            throw new BadRequestException(Message.LOGIN_00004);
        
        //Genera un token JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JWT:Key"]);
        
        // Define el tiempo de expiración en función de la opción "Remember Me"
        var tokenExpiration = rememberMe ? TimeSpan.FromDays(30) : TimeSpan.FromHours(1);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UsuarioId.ToString()),
                new Claim(ClaimTypes.Email, user.Username)
            }),
            Expires = DateTime.UtcNow.Add(tokenExpiration),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        var response = new JWTSettings()
        {
            Token = tokenHandler.WriteToken(token),
            Expiracion = token.ValidTo
        };

        return response;
    }
}

public record JWTSettings
{
    public string Token { get; set; }
    public DateTime Expiracion { get; set; }
}