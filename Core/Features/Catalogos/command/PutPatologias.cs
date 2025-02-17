using System.ComponentModel.DataAnnotations;
using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Catalogos.command;

public record PutPatologias : IRequest
{
    public string PatologiasId { get; set; }
    public string? Descripcion { get; set; }
    public bool? Status { get; set; }
}

public class PutPatologiasHandler : IRequestHandler<PutPatologias>
{
    private readonly FisioContext _context;
    
    public PutPatologiasHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task Handle(PutPatologias request, CancellationToken cancellationToken)
    {
        var patologias = await _context.Patologias
            .FindAsync(request.PatologiasId.HashIdInt())
            ?? throw new NotFoundException(Message.CAT_0004);
        
        patologias.Descripcion = request.Descripcion ?? patologias.Descripcion;
        patologias.Status = request.Status ?? patologias.Status;
        
        _context.Patologias.Update(patologias);
        await _context.SaveChangesAsync();
    }
}