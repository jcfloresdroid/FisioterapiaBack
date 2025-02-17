using Core.Domain.Exceptions;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using Core.Services.Interfaz.Validator;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Features.Usuario.command;

public record ModificarFoto : IRequest
{
    public IFormFile Foto { get; set; }
}

public class ModificarFotoHandler : IRequestHandler<ModificarFoto>
{
    private readonly FisioContext _context;
    private readonly IConvertType _convertType;
    private readonly IAuthorization _authorization;
    private readonly IUsuarioValidator _validator;
    
    public ModificarFotoHandler(FisioContext context, IConvertType convertType, 
        IAuthorization authorization, IUsuarioValidator validator)
    {
        _context = context;
        _convertType = convertType;
        _authorization = authorization;
        _validator = validator;
    }
    
    public async Task Handle(ModificarFoto request, CancellationToken cancellationToken)
    {
        // Validacion
        await _validator.ModificarFoto(request);
        
        var user = await _context.Usuarios
            .FindAsync(_authorization.UsuarioActual()) 
            ?? throw new NotFoundException(Message.USER_00001);
        
        user.FotoPerfil = await _convertType.uploadFile(request.Foto);
        
        _context.Usuarios.Update(user);
        await _context.SaveChangesAsync();
    }
}