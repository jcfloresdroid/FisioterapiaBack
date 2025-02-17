
using Application.Core.Domain.Exceptions;
using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;

namespace Core.Features.Fisioterapeutas.command;

public record StatusFisios() : IRequest
{
    public string FisioId { get; set; }
};

public class StatusFisiosHandler : IRequestHandler<StatusFisios>
{
    private readonly FisioContext _context;
    
    public StatusFisiosHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task Handle(StatusFisios request, CancellationToken cancellationToken)
    {
        if(request.FisioId == null) 
            throw new BadRequestException(Message.FIS_0008);
        
        var fisio = await _context.Fisioterapeuta
            .FindAsync(request.FisioId.HashIdInt()) 
            ?? throw new NotFoundException(Message.FIS_0001);
        
        fisio.Status = !fisio.Status;

        _context.Fisioterapeuta.Update(fisio);
        await _context.SaveChangesAsync();
    }
}