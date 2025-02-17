using System.ComponentModel.DataAnnotations;
using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Catalogos.command;

public record PutAnticonceptivo : IRequest
{
    public string AnticonceptivoId { get; set; }
    public string? Descripcion { get; set; }
    public bool? Status { get; set; }
}

public class PutAnticonceptivoHandler : IRequestHandler<PutAnticonceptivo>
{
    private readonly FisioContext _context;
    
    public PutAnticonceptivoHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task Handle(PutAnticonceptivo request, CancellationToken cancellationToken)
    {
        var anticonceptivo = await _context.TipoAnticonceptivos
            .FindAsync(request.AnticonceptivoId.HashIdInt()) 
            ?? throw new NotFoundException(Message.CAT_0006);
        
        anticonceptivo.Descripcion = request.Descripcion ?? anticonceptivo.Descripcion;
        anticonceptivo.Status = request.Status ?? anticonceptivo.Status;
        
        _context.TipoAnticonceptivos.Update(anticonceptivo);
        await _context.SaveChangesAsync();
    }
}