using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using iTextSharp.text;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Graficos.queries;

public record GraphicsMotivoAlta() : IRequest<List<GraphicsMotivoAltaResponse>>
{
    public int Meses { get; set; }
};

public class GraphicsMotivoAltaHandler : IRequestHandler<GraphicsMotivoAlta, List<GraphicsMotivoAltaResponse>>
{
    private readonly FisioContext _context;
    
    public GraphicsMotivoAltaHandler(FisioContext context)
    {
        _context = context;
    }

    public async Task<List<GraphicsMotivoAltaResponse>> Handle(GraphicsMotivoAlta request, CancellationToken cancellationToken)
    {
        var startDate = FormatDate.DateLocal().AddMonths(-request.Meses);

        var motivoAlta = await _context.Diagnosticos
            .Where(d => d.FechaAlta >= startDate)
            .GroupBy(d => d.MotivoAlta.Descripcion)
            .Select(d => new
            {
                MotivoAlta = d.Key,
                Count = d.Count()
            })
            .ToListAsync(cancellationToken);

        var response = motivoAlta.Select(x => new GraphicsMotivoAltaResponse()
        {
            Nombre = x.MotivoAlta,
            Total = x.Count
        }).ToList();

        return response;
    }
}

public record GraphicsMotivoAltaResponse()
{
    public string Nombre { get; set; }
    public int Total { get; set; }
};