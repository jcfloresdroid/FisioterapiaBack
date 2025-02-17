using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Diagnostico.queries;

public record GetRevisiones() : IRequest<List<GetRevisionesResponse>>
{
    public string DiagnosticoId { get; set; }
}

public class GetRevisionesHandler : IRequestHandler<GetRevisiones, List<GetRevisionesResponse>>
{
    private readonly FisioContext _context;
    
    public GetRevisionesHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task<List<GetRevisionesResponse>> Handle(GetRevisiones request, CancellationToken cancellationToken)
    {
        var revisiones = await _context.Revisions
            .AsNoTracking()
            .Include(x => x.ExploracionFisica)
            .Include(x => x.Servicio)
            .OrderByDescending(x => x.Fecha)
            .ThenByDescending(x => x.Hora)
            .ThenByDescending(x => x.RevisionId)
            .Where(x => x.DiagnosticoId == request.DiagnosticoId.HashIdInt())
            .ToListAsync();

        return await Task.FromResult(revisiones.Select(x => new GetRevisionesResponse()
        {
            Notas = x.Notas,
            FolioPago = x.FolioPago,
            ServicioId = x.Servicio.Descripcion,
            Fecha = x.Fecha,
            Hora = x.Hora,
            ExploracionFisica = new ExplorationRevisionGet()
            {
                Fr = x.ExploracionFisica.Fr,
                Fc = x.ExploracionFisica.Fc,
                Temperatura = x.ExploracionFisica.Temperatura,
                Peso = x.ExploracionFisica.Peso,
                Estatura = x.ExploracionFisica.Estatura,
                Imc = x.ExploracionFisica.Imc,
                IndiceCinturaCadera = x.ExploracionFisica.IndiceCinturaCadera,
                SaturacionOxigeno = x.ExploracionFisica.SaturacionOxigeno,
                PresionArterial = x.ExploracionFisica.PresionArterial
            }
        }).ToList()); 
    }
}

public record GetRevisionesResponse()
{
    public string Notas { get; set; }

    public string FolioPago { get; set; }
    
    public string ServicioId { get; set; }
    
    public DateTime Fecha { get; set; }
    
    public TimeSpan Hora { get; set; }
    
    public ExplorationRevisionGet ExploracionFisica { get; set; }
}

public record ExplorationRevisionGet()
{
    public int? Fr { get; set; }
    
    public int? Fc { get; set; }
 
    public float? Temperatura { get; set; }
    
    public float? Peso { get; set; }
    
    public float? Estatura { get; set; }
    
    public float? Imc { get; set; }
    
    public float? IndiceCinturaCadera { get; set; }
    
    public float? SaturacionOxigeno { get; set; }
    
    public string PresionArterial { get; set; }
}