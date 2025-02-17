using Application.Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Domain.Entities;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using Core.Services.Interfaz.Validator;
using Core.Validator.Usuario;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Features.Usuario.command;

public record CreateUser : IRequest
{
    public string Username { get; set; }
    public string Contraseña { get; set; }
    public string EspecialidadId { get; set; }
    public IFormFile? Foto { get; set; }
};

public class CreateUserHandler : IRequestHandler<CreateUser>
{
    private readonly FisioContext _context;
    private readonly IConvertType _convertType;
    private readonly IExistResource _existResource;
    private readonly IUsuarioValidator _validator;
    
    public CreateUserHandler(FisioContext context, IConvertType convertType, IExistResource existResource, IUsuarioValidator validator)
    {
        _context = context;
        _validator = validator;
        _convertType = convertType;
        _existResource = existResource;
    }

    public async Task Handle(CreateUser request, CancellationToken cancellationToken)
    {
        // Validacion
        await _validator.CreateUser(request);
        
        // Buscamos que existe la especialidad
        await _existResource.ExistEspecialidad(request.EspecialidadId);
        
        var user = new Domain.Entities.Usuario()
        {
            Username = request.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Contraseña),
            Clave = BCrypt.Net.BCrypt.HashPassword("12345"),
            EspecialidadId = request.EspecialidadId.HashIdInt(),
            FotoPerfil = request.Foto == null ? await _convertType.profilePicture() : await _convertType.uploadFile(request.Foto)
        };
        
        await _context.Usuarios.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}