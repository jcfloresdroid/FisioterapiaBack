using System.Security.Cryptography;
using Core.Domain.Exceptions;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Cuenta.queries;

public record TokenRefresh() : IRequest<TokenRefreshResponse>
{
    public string Refresh { get; set; }
};

public class TokenRefreshHandler : IRequestHandler<TokenRefresh, TokenRefreshResponse>
{
    private readonly FisioContext _context;
    private readonly IAuthService _authService;
    
    public TokenRefreshHandler(FisioContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<TokenRefreshResponse> Handle(TokenRefresh request, CancellationToken cancellationToken)
    {
        // Verificamos que exista el token y no halla caducado
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == request.Refresh && x.Expiracion > DateTime.UtcNow)
            ?? throw new UnauthorizedException(Message.TOKEN_00001);

        var user = await _context.Usuarios
            .FindAsync(token.UsuarioId)
            ?? throw new NotFoundException(Message.USER_00001);
        
        var generarToken = await _authService.AuthenticateAsync(user.Username, user.Password, true);
        
        var response = new TokenRefreshResponse()
        {
            Success = true,
            AccessToken = generarToken.Token,
            RefreshToken = GenerateRefreshToken(),
            Vigencia = generarToken.Expiracion,
        };

        token.Token = response.RefreshToken;
        token.Expiracion = response.Vigencia;
        
        _context.RefreshTokens.Update(token);
        await _context.SaveChangesAsync();
        
        return response;
    }
    
    private string GenerateRefreshToken() {
        using (var rng = new RNGCryptoServiceProvider()) {
            var randomBytes = new byte[32];
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}

public record TokenRefreshResponse()
{
    public bool Success { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Vigencia { get; set; }
};
