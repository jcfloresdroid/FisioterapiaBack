using Core.Domain.Exceptions;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Usuario.queries;

public record ClaveAdmin : IRequest<ClaveAdminResponse>
{
    public string Clave { get; set; }
}

public class ClaveAdminHandler : IRequestHandler<ClaveAdmin, ClaveAdminResponse>
{
    private readonly FisioContext _context;
    private readonly IAuthorization _authorization;
    
    public ClaveAdminHandler(FisioContext context, IAuthorization authorization)
    {
        _context = context;
        _authorization = authorization;
    }
    
    public async Task<ClaveAdminResponse> Handle(ClaveAdmin request, CancellationToken cancellationToken)
    {
        var clave = await _context.Usuarios
            .FindAsync(_authorization.UsuarioActual())
            ?? throw new NotFoundException(Message.USER_00001);
        
        //Se compara la contraseña para verificar que sean las mismas
        var password = BCrypt.Net.BCrypt.Verify(request.Clave, clave.Clave);
        
        return await Task.FromResult(new ClaveAdminResponse()
        {
            isAdmin = password
        });
    }
}

public record ClaveAdminResponse
{
    public bool isAdmin { get; set; }
}