using System.Security.Cryptography;
using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Cuenta.command;

public record Login : IRequest<LoginResponse> {
    public string Username { get; set; }
    public string Contraseña { get; set; }
    public bool Recordarme { get; set; } = false;
};

public class LoginHandler : IRequestHandler<Login, LoginResponse> 
{
    private readonly FisioContext _context;
    private readonly IAuthService _authService;
    
    public LoginHandler(FisioContext context, IAuthService authService) {
        _context = context;
        _authService = authService;
    }

    public async Task<LoginResponse> Handle(Login request, CancellationToken cancellationToken) {
        //Valida que no esten vacias
        if(string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Contraseña))
            throw new BadRequestException(Message.LOGIN_00003);
        
        //Se busca al usuario propietario del username
        var user = await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == request.Username)
            ?? throw new NotFoundException(Message.LOGIN_00002);
        
        //Se compara la contraseña para verificar que sean las mismas
        var password = BCrypt.Net.BCrypt.Verify(request.Contraseña, user.Password) ? user.Password : request.Contraseña;
        
        //Si cumple con las validaciones se procede a autenticar
        var token = await _authService.AuthenticateAsync(request.Username, password, request.Recordarme);
        
        var response = new LoginResponse()
        {
            Success = true,
            AccessToken = token.Token,
            RefreshToken = request.Recordarme ? GenerateRefreshToken() : null,
            Vigencia = token.Expiracion,
            Usuario = new UserDate()
            {
                Id = user.UsuarioId.HashId(),
                Username = user.Username,
                Foto = user.FotoPerfil
            }
        };

        if (request.Recordarme) {
            var validation = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.UsuarioId == user.UsuarioId);

            //Si el existe ya el usuario con un refresh token solo que lo actualice
            if (validation == null) {
                var refresh = new RefreshToken() {
                    UsuarioId = user.UsuarioId,
                    Token = response.RefreshToken,
                    Expiracion = response.Vigencia
                };
            
                await _context.RefreshTokens.AddAsync(refresh);
                await _context.SaveChangesAsync();
            } else {
                validation.Token = response.RefreshToken;
                validation.Expiracion = response.Vigencia;
                
                _context.RefreshTokens.Update(validation);
                await _context.SaveChangesAsync();
            }
        }

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

public record LoginResponse
{
    public bool Success { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Vigencia { get; set; }
    public UserDate Usuario { get; set; }
}

public record UserDate
{
    public string Id { get; set; }
    public string Username { get; set; }
    public byte[] Foto { get; set; }
}