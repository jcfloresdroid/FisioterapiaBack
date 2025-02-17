using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Diagnostico.queries;

public record DiagnosticGet : IRequest<GeneralDiagnosticResponse>
{
    public string DiagnosticoId { get; set; }
}

public record DiagnosticGetHandler : IRequestHandler<DiagnosticGet, GeneralDiagnosticResponse>
{
    private readonly FisioContext _context;
    
    public DiagnosticGetHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task<GeneralDiagnosticResponse> Handle(DiagnosticGet request, CancellationToken cancellationToken)
    {
        var diagnostic = await _context.Diagnosticos
            .AsNoTracking()
            .Include(x => x.ProgramaFisioterapeutico)
            .Include(x => x.MapaCorporal)
            .FirstOrDefaultAsync(x => x.DiagnosticoId == request.DiagnosticoId.HashIdInt())
            ?? throw new NotFoundException("No se encontro el diagnostico");
        
        //Buscamos el ultimo registro de exploracion fisica
        var revision = await _context.Revisions
            .AsNoTracking()
            .Include(x => x.ExploracionFisica)
            .Where(x => x.DiagnosticoId == diagnostic.DiagnosticoId)
            .OrderByDescending(x => x.Fecha)
            .ThenByDescending(x => x.RevisionId)
            .FirstOrDefaultAsync();
        
        return await Task.FromResult(new GeneralDiagnosticResponse
        {
            Diagnostico = diagnostic.Descripcion,
            UltimaRevision = revision.Fecha,
            diagnostic = new GetDiagnostic
            {
                Refiere = diagnostic.Refiere,
                Categoria = diagnostic.Categoria,
                DiagnosticoPrevio = diagnostic.DiagnosticoPrevio,
                TerapeuticaEmpleada = diagnostic.TerapeuticaEmpleada,
                DiagnosticoFuncional = diagnostic.DiagnosticoFuncional,
                PadecimientoActual = diagnostic.PadecimientoActual,
                Inspeccion = diagnostic.Inspeccion,
                ExploracionFisicaDescripcion = diagnostic.ExploracionFisicaCuadro,
                EstudiosComplementarios = diagnostic.EstudiosComplementarios,
                DiagnosticoNosologico = diagnostic.DiagnosticoNosologico,
                FechaInicio = diagnostic.FechaInicio,
                FechaAlta = diagnostic.FechaAlta,
                DiagnosticoInicial = diagnostic.DiagnosticoInicial,
                DiagnosticoFinal = diagnostic.DiagnosticoFinal,
                FrecuenciaTratamiento = diagnostic.FrecuenciaTratamiento,
                MotivoAltaId = (diagnostic.MotivoAltaId ?? default(int)).HashId(),
                Estatus = diagnostic.Estatus
            },
            program = new ProgramGet
            {
                CortoPlazo = diagnostic.ProgramaFisioterapeutico.CortoPlazo,
                MedianoPlazo = diagnostic.ProgramaFisioterapeutico.MedianoPlazo,
                LargoPlazo = diagnostic.ProgramaFisioterapeutico.LargoPlazo,
                TratamientoFisioterapeutico = diagnostic.ProgramaFisioterapeutico.TratamientoFisioterapeutico,
                Sugerencias = diagnostic.ProgramaFisioterapeutico.Sugerencias,
                Pronostico = diagnostic.ProgramaFisioterapeutico.Pronostico
            },
            map = new MapGet
            {
                valores = diagnostic.MapaCorporal.Valor,
                RangoDolor = diagnostic.MapaCorporal.RangoDolor,
                Nota = diagnostic.MapaCorporal.Nota
            },
            exploration = new ExplorationGet
            {
                Temperatura = revision.ExploracionFisica.Temperatura,
                Fr = revision.ExploracionFisica.Fr,
                Fc = revision.ExploracionFisica.Fc,
                PresionArterial = revision.ExploracionFisica.PresionArterial,
                Peso = revision.ExploracionFisica.Peso,
                Estatura = revision.ExploracionFisica.Estatura,
                Imc = revision.ExploracionFisica.Imc,
                IndiceCinturaCadera = revision.ExploracionFisica.IndiceCinturaCadera,
                SaturacionOxigeno = revision.ExploracionFisica.SaturacionOxigeno
            }
        });
    }
}

public record GeneralDiagnosticResponse
{
    public string Diagnostico { get; set; }
    
    public DateTime UltimaRevision { get; set; }
    
    public GetDiagnostic diagnostic { get; set; }
    public ProgramGet program { get; set; }
    public MapGet map { get; set; }
    public ExplorationGet exploration { get; set; }
}

public record GetDiagnostic()
{
    public string Refiere { get; set; }
    
    public string Categoria { get; set; }
    
    public string DiagnosticoPrevio { get; set; }
    
    public string TerapeuticaEmpleada { get; set; }
    
    public string DiagnosticoFuncional { get; set; }
    
    public string PadecimientoActual { get; set; }
    
    public string Inspeccion { get; set; }
    
    public string ExploracionFisicaDescripcion { get; set; }
    
    public string EstudiosComplementarios { get; set; }
    
    public string DiagnosticoNosologico { get; set; }
    
    public DateTime FechaInicio { get; set; }

    public DateTime? FechaAlta { get; set; }
    
    public string? DiagnosticoInicial { get; set; }

    public string? DiagnosticoFinal { get; set; }

    public string? FrecuenciaTratamiento { get; set; }
    
    public string MotivoAltaId { get; set; }
    
    public bool Estatus { get; set; }
}

public record ProgramGet()
{
    public string CortoPlazo { get; set; }
    
    public string MedianoPlazo { get; set; }
    
    public string LargoPlazo { get; set; }
    
    public string TratamientoFisioterapeutico { get; set; }
    
    public string Sugerencias { get; set; }
    
    public string Pronostico { get; set; }
}

public record MapGet()
{
    public List<int> valores { get; set; }
    
    public List<int> RangoDolor { get; set; }
    
    public string Nota { get; set; }
}

public record ExplorationGet()
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