using Application.Core.Domain.Exceptions;
using Core.Domain.Exceptions;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using Core.Validator.Cuenta;
using MediatR;

namespace Core.Features.Cuenta.command;

public record ModificarContraseñas : IRequest
{
    public string ContraseñaActual { get; set; }
    public string NuevaContraseña { get; set; }
    public string ConfirmarContraseña { get; set; }
}

public class ModificarContraseñasHandler : IRequestHandler<ModificarContraseñas>
{
    private readonly FisioContext _context;
    private readonly IAuthorization _authorization;
    private readonly ContraseñaValidator _contraseñaValidator;

    public ModificarContraseñasHandler(FisioContext context, IAuthorization authorization,
        ContraseñaValidator contraseñaValidator)
    {
        _context = context;
        _authorization = authorization;
        _contraseñaValidator = contraseñaValidator;
    }

    public async Task Handle(ModificarContraseñas request, CancellationToken cancellationToken)
    {
        var passValidator = await _contraseñaValidator.ValidateAsync(request, cancellationToken);

        if (!passValidator.IsValid)
            throw new ValidationException(passValidator.Errors);

        var usuario = await _context.Usuarios
            .FindAsync(_authorization.UsuarioActual())
            ?? throw new BadRequestException(Message.USER_00001);

        // Verificamos que las contraseñan coincidan
        if (request.NuevaContraseña != request.ConfirmarContraseña)
            throw new BadRequestException(Message.MPASS_0004);

        // Verificamos que la contraseña si sea la correcta
        var password = BCrypt.Net.BCrypt.Verify(request.ContraseñaActual, usuario.Password);

        if (!password)
            throw new BadRequestException(Message.MPASS_0005);

        usuario.Password = BCrypt.Net.BCrypt.HashPassword(request.NuevaContraseña);

        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }
}