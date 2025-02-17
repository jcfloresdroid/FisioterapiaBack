using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features;

public record SelectFisio( ) : IRequest<List<SelectFisioResponse>>;

public class SelectFisioHandler : IRequestHandler<SelectFisio, List<SelectFisioResponse>>
{
    private readonly FisioContext _context;
    
    public SelectFisioHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task<List<SelectFisioResponse>> Handle(SelectFisio request, CancellationToken cancellationToken)
    {
        var fisio = await _context.Fisioterapeuta
            .Where(x => x.Status == true)
            .Select(f => new SelectFisioResponse
            {
                FisioId = f.FisioterapeutaId.HashId(),
                Nombre = f.Nombre
            })
            .ToListAsync(cancellationToken);
 
        return fisio;
    }
}

public record SelectFisioResponse()
{
    public string FisioId { get; set; }
    public string Nombre { get; set; }
}