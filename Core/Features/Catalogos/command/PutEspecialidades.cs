using System.ComponentModel.DataAnnotations;
using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Catalogos.command;

public record PutEspecialidades : IRequest
{
    public string EspecialidadId { get; set; }
    public string? Descripcion { get; set; }
    public bool? Status { get; set; }
}

public class PutEspecialidadesHandler : IRequestHandler<PutEspecialidades>
{
    private readonly FisioContext _context;
    
    public PutEspecialidadesHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task Handle(PutEspecialidades request, CancellationToken cancellationToken)
    {
        var especialidades = await _context.Especialidades
            .FindAsync(request.EspecialidadId.HashIdInt()) 
            ?? throw new NotFoundException(Message.CAT_0001);
        
        especialidades.Descripcion = request.Descripcion ?? especialidades.Descripcion;
        especialidades.Status = request.Status ?? especialidades.Status;
        
        _context.Especialidades.Update(especialidades);
        await _context.SaveChangesAsync();
    }
}