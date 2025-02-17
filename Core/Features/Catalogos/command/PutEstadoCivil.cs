using System.ComponentModel.DataAnnotations;
using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Catalogos.command;

public record PutEstadoCivil : IRequest
{
    public string EstadoCivilId { get; set; }
    public string? Descripcion { get; set; }
    public bool? Status { get; set; }
}

public class PutEstadoCivilHandler : IRequestHandler<PutEstadoCivil>
{
    private readonly FisioContext _context;
    
    public PutEstadoCivilHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task Handle(PutEstadoCivil request, CancellationToken cancellationToken)
    {
        var estado = await _context.EstadoCivils
            .FindAsync(request.EstadoCivilId.HashIdInt()) 
            ?? throw new NotFoundException(Message.CAT_0002);
        
        estado.Descripcion = request.Descripcion ?? estado.Descripcion;
        estado.Status = request.Status ?? estado.Status;
        
        _context.EstadoCivils.Update(estado);
        await _context.SaveChangesAsync();
    }
}