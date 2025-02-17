using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Diagnostico.queries;

public record GetUltimaExploracion : IRequest<GetUltimaExploracionResponse>
{
    public string PacienteId { get; set; }
}

public class GetUltimoExploracionHandler : IRequestHandler<GetUltimaExploracion, GetUltimaExploracionResponse>
{
    private readonly FisioContext _context;
    
    public GetUltimoExploracionHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task<GetUltimaExploracionResponse> Handle(GetUltimaExploracion request, CancellationToken cancellationToken)
    {
        //Buscamos el ultimo registro de exploracion fisica
        var revision = await _context.Revisions
            .AsNoTracking()
            .Include(x => x.ExploracionFisica)
            .Where(x => x.Diagnostico.Expediente.paciente.PacienteId == request.PacienteId.HashIdInt())
            .OrderByDescending(x => x.Fecha)
            .ThenByDescending(x => x.RevisionId)
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException("No se encontro una exploracion fisica para el paciente");
        
        return await Task.FromResult(new GetUltimaExploracionResponse()
        {
            UltimoRegistro = revision.Fecha,
            Fr = revision.ExploracionFisica.Fr,
            Fc = revision.ExploracionFisica.Fc,
            Temperatura = revision.ExploracionFisica.Temperatura,
            Peso = revision.ExploracionFisica.Peso,
            Estatura = revision.ExploracionFisica.Estatura,
            Imc = revision.ExploracionFisica.Imc,
            IndiceCinturaCadera = revision.ExploracionFisica.IndiceCinturaCadera,
            SaturacionOxigeno = revision.ExploracionFisica.SaturacionOxigeno,
            PresionArterial = revision.ExploracionFisica.PresionArterial
        });
    }
}

public record GetUltimaExploracionResponse()
{
    public DateTime UltimoRegistro { get; set; }
    
    public int Fr { get; set; }
    
    public int Fc { get; set; }
 
    public float Temperatura { get; set; }
    
    public float Peso { get; set; }
    
    public float Estatura { get; set; }
    
    public float Imc { get; set; }
    
    public float IndiceCinturaCadera { get; set; }
    
    public float SaturacionOxigeno { get; set; }
    
    public string PresionArterial { get; set; }
}