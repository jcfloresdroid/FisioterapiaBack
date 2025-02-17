using Core.Domain.Exceptions;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using MediatR;

namespace Core.Features.Cuenta.command;

public record Logout() : IRequest {}

public class LogoutHandler : IRequestHandler<Logout>
{
    private readonly IAuthorization _authorization;
    private readonly FisioContext _context;
    
    public LogoutHandler(IAuthorization authorization, FisioContext context)
    {
        _authorization = authorization;
        _context = context;
    }

    public async Task Handle(Logout request, CancellationToken cancellationToken)
    {
        var refreshToken = await _context.RefreshTokens
            .FindAsync(_authorization.UsuarioActual())
            ?? throw new NotFoundException(Message.LOGOUT_00001);
        
        _context.RefreshTokens.Remove(refreshToken);
        await _context.SaveChangesAsync();
    }
}