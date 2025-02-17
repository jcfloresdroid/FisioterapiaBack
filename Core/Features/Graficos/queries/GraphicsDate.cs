using Core.Domain.Enum;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Graficos.queries;

public record GraphicsDate : IRequest<GraphicsDateResponse>
{
    
}

public class GraphicsDateHandler : IRequestHandler<GraphicsDate, GraphicsDateResponse>
{
    private readonly FisioContext _context;
    
    public GraphicsDateHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task<GraphicsDateResponse> Handle(GraphicsDate request, CancellationToken cancellationToken)
    {
        var citas = await _context.Citas
            .AsNoTracking()
            .Where(x => x.Fecha.Date >= FormatDate.StartOfWeek().Date && x.Fecha.Date <= FormatDate.EndOfWeek().Date)
            .ToListAsync();
        
        return await Task.FromResult(new GraphicsDateResponse()
        {
            Concluidados = citas.Count(x => x.Status == (int)EstadoCita.Concluida),
            Inasistencias = citas.Count(x => x.Status == (int)EstadoCita.Inasistencia),
            Pendientes = citas.Count(x => x.Status == (int)EstadoCita.Pendiente),
            Cancelados = citas.Count(x => x.Status == (int)EstadoCita.Cancelada),
            Total = citas.Count
        });
    }
}

public record GraphicsDateResponse
{
    public int Concluidados { get; set; }
    public int Inasistencias { get; set; }
    public int Pendientes { get; set; }
    public int Cancelados { get; set; }
    public int Total { get; set; }
}


