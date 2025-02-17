using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using Core.Services.Interfaz.Validator;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Features.Usuario.command;

public record ModificarUsuario() : IRequest
{
    public string? Nombre { get; set; }
    public string? Correo { get; set; }
    public string? Telefono { get; set; }
    public string? EspecialidadId { get; set; }
    public IFormFile? Foto { get; set; }
};

public class ModificarUsuarioHandler : IRequestHandler<ModificarUsuario>
{
    private readonly FisioContext _context;
    private readonly IAuthorization _authorization;
    private readonly IExistResource _existResource;
    private readonly IUsuarioValidator _validator;
    private readonly IConvertType _convertType;
    
    public ModificarUsuarioHandler(FisioContext context, IAuthorization authorization, 
        IExistResource existResource, IUsuarioValidator validator, IConvertType convertType)
    {
        _context = context;
        _authorization = authorization;
        _existResource = existResource;
        _validator = validator;
        _convertType = convertType;
    }
    
    public async Task Handle(ModificarUsuario request, CancellationToken cancellationToken)
    {
        // Validacion
        await _validator.ModificarUsuario(request);
        
        // Validar si la especialidad existe
        if(request.EspecialidadId != null)
            await _existResource.ExistEspecialidad(request.EspecialidadId);
        
        var usuario = await _context.Usuarios
            .FindAsync(_authorization.UsuarioActual())
            ?? throw new NotFoundException(Message.USER_00001);
        
        usuario.Username = request.Nombre ?? usuario.Username;
        usuario.Correo = request.Correo ?? usuario.Correo;
        usuario.Telefono = request.Telefono ?? usuario.Telefono;
        usuario.EspecialidadId = request.EspecialidadId == null ? usuario.EspecialidadId : request.EspecialidadId.HashIdInt();
        usuario.FotoPerfil = request.Foto == null ? usuario.FotoPerfil : await _convertType.uploadFile(request.Foto);
        
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync(cancellationToken);
    }
}