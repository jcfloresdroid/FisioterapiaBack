using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities;
using Core.Domain.Enum;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using Core.Services.Implementacion.Validator;
using Core.Services.Interfaz;
using Core.Services.Interfaz.Validator;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Features.Fisioterapeutas.command;

public record PostFisioterapeutas : IRequest
{
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Telefono { get; set; }
    public string EspecialidadId { get; set; }
    public string? Cedula { get; set; }
    public IFormFile? Foto { get; set; }
}

public class PostFisioterapeutasHandler : IRequestHandler<PostFisioterapeutas>
{
    private readonly FisioContext _context;
    private readonly IConvertType _convertType;
    private readonly IExistResource _existResource;
    private readonly IFisioValidator _validator;

    public PostFisioterapeutasHandler(FisioContext context, IConvertType convertType, IExistResource existResource, IFisioValidator validator)
    {
        _context = context;
        _validator = validator;
        _convertType = convertType;
        _existResource = existResource;
    }
    
    public async Task Handle(PostFisioterapeutas request, CancellationToken cancellationToken)
    {
        // Validacion
        await _validator.CreateFisio(request);
        
        // Buscamos que existe la especialidad
        await _existResource.ExistEspecialidad(request.EspecialidadId);
        
        var fisio = new Fisioterapeuta() {
            Nombre = request.Nombre,
            Correo = request.Correo,
            Telefono = request.Telefono,
            CedulaProfesional = request.Cedula,
            Status = Convert.ToBoolean((int)Estatus.Activo),
            EspecialidadId = request.EspecialidadId.HashIdInt(),
            FotoPerfil = request.Foto == null ? await _convertType.profilePicture() : await _convertType.uploadFile(request.Foto)
        };

        await _context.Fisioterapeuta.AddAsync(fisio);
        await _context.SaveChangesAsync();
    }
}