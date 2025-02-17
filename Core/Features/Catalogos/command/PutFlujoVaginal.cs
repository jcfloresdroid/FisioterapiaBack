using System.ComponentModel.DataAnnotations;
using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Catalogos.command;

public record PutFlujoVaginal : IRequest
{
    public string FlujoVaginalId { get; set; }
    public string? Descripcion { get; set; }
    public bool? Status { get; set; }
}

public class PutFlujoVaginalHandler : IRequestHandler<PutFlujoVaginal>
{
    private readonly FisioContext _context;
    
    public PutFlujoVaginalHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task Handle(PutFlujoVaginal request, CancellationToken cancellationToken)
    {
        var flujo = await _context.FlujoVaginals
            .FindAsync(request.FlujoVaginalId.HashIdInt()) 
            ?? throw new NotFoundException(Message.CAT_0003);
        
        flujo.Descripcion = request.Descripcion ?? flujo.Descripcion;
        flujo.Status = request.Status ?? flujo.Status;
        
        _context.FlujoVaginals.Update(flujo);
        await _context.SaveChangesAsync();
    }
}