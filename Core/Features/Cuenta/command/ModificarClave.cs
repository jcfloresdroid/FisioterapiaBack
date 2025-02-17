using Application.Core.Domain.Exceptions;
using Core.Domain.Exceptions;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using Core.Validator.Cuenta;
using MediatR;

namespace Core.Features.Cuenta.command;

public record ModificarClave : IRequest
{
    public string ClaveActual { get; set; }
    public string ClaveNueva { get; set; }
    public string ConfirmarClave { get; set; }
}

public class ModificarClaveHandler : IRequestHandler<ModificarClave>
{
    private readonly FisioContext _context;
    private readonly IAuthorization _authorization;
    private readonly ClaveValidator _claveValidator;

    public ModificarClaveHandler(FisioContext context, IAuthorization authorization, ClaveValidator claveValidator)
    {
        _context = context;
        _authorization = authorization;
        _claveValidator = claveValidator;
    }

    public async Task Handle(ModificarClave request, CancellationToken cancellationToken)
    {
        var passValidator = await _claveValidator.ValidateAsync(request, cancellationToken);

        if (!passValidator.IsValid)
            throw new ValidationException(passValidator.Errors);

        var usuario = await _context.Usuarios
            .FindAsync(_authorization.UsuarioActual())
            ?? throw new BadRequestException(Message.USER_00001);

        // Verificamos que las claves coincidan
        if (request.ClaveNueva != request.ConfirmarClave)
            throw new BadRequestException(Message.MCLAVE_0004);

        // Verificamos que la clave si sea la correcta
        var clave = BCrypt.Net.BCrypt.Verify(request.ClaveActual, usuario.Clave);

        if (!clave)
            throw new BadRequestException(Message.MCLAVE_0005);

        usuario.Clave = BCrypt.Net.BCrypt.HashPassword(request.ClaveNueva);
        
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }
}