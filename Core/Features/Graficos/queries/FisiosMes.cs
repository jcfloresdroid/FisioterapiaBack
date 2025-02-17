using Core.Domain.Enum;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Graficos.queries;

public record FisiosMes() : IRequest<List<FisioMesResponse>>;

public class FisioMesHandler : IRequestHandler<FisiosMes, List<FisioMesResponse>>
{
    private readonly FisioContext _context;
    
    public FisioMesHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task<List<FisioMesResponse>> Handle(FisiosMes request, CancellationToken cancellationToken)
    {
        DateTime StartMonth = new DateTime(FormatDate.DateLocal().Year, FormatDate.DateLocal().Month, 1);
        DateTime EndMonth = StartMonth.AddMonths(1).AddDays(-1);
        
        var cita = await _context.Citas
            .Include(x => x.Paciente)
            .Include(x => x.Fisio)
            .Where(c => c.Fecha >= StartMonth && c.Fecha <= EndMonth && c.Status == (int)EstadoCita.Concluida)
            .GroupBy(c => c.Fisio.Nombre)
            .Select(c => new
            {
                Fisioterapeuta = c.Key,
                Foto = c.Select(x => x.Fisio.FotoPerfil).FirstOrDefault(),
                Total = c.Count()
            })
            .OrderByDescending(c => c.Total)
            .Take(3)
            .ToListAsync();

        var response = cita.Select(c => new FisioMesResponse
        {
            Nombre = c.Fisioterapeuta,
            Foto = c.Foto,
            Total = c.Total
        }).ToList();

        return response;
    }
}

public record FisioMesResponse()
{
    public string Nombre { get; set; }
    public byte[] Foto { get; set; }
    public int Total { get; set; }
}