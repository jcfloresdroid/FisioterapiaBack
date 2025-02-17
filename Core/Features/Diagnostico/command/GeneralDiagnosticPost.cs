using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using Core.Services.Interfaz.Validator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Diagnostico.command;

public record ExplorationPost()
{
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

public record MapPost()
{
    public List<int> valores { get; set; }
    public List<int> RangoDolor { get; set; }
    public string Nota { get; set; }
}

public record PostDiagnostic()
{
    public string Descripcion { get; set; }
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
}

public record ProgramPost()
{
    public string CortoPlazo { get; set; }
    public string MedianoPlazo { get; set; }
    public string LargoPlazo { get; set; }
    public string TratamientoFisioterapeutico { get; set; }
    public string Sugerencias { get; set; }
    public string Pronostico { get; set; }
}

public record ReviewPost()
{
    public string Notas { get; set; }
    public string FolioPago { get; set; }
    public string ServicioId { get; set; }
}

public record GeneralDiagnosticPost() : IRequest
{
    public string ExpedienteId { get; set; }
    public ExplorationPost Exploration { get; set; }
    public MapPost Map { get; set; }
    public PostDiagnostic Diagnostic { get; set; }
    public ProgramPost Program { get; set; }
    public ReviewPost Review { get; set; }
}

public class PostDiagnosticHanlder : IRequestHandler<GeneralDiagnosticPost>
{
    private readonly FisioContext _context;
    private readonly IExistResource _existResource;
    private readonly IDiagnosticoValidator _validator;

    public PostDiagnosticHanlder(FisioContext context, IExistResource existResource, IDiagnosticoValidator validator)
    {
        _context = context;
        _existResource = existResource;
        _validator = validator;
    }
    
    public async Task Handle(GeneralDiagnosticPost request, CancellationToken cancellationToken)
    {
        // Validacion
        await _validator.crearDiagnostico(request);
        
        // Existencias
        await _existResource.ExistExpediente(request.ExpedienteId);
        await _existResource.ExistServicio(request.Review.ServicioId);

        var diagnostic = await _context.Diagnosticos
            .Where(x => x.ExpedienteId == request.ExpedienteId.HashIdInt() && x.Estatus)
            .FirstOrDefaultAsync();
        
        if (diagnostic != null)
            throw new BadRequestException(Message.DIAG_0014);
        
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                // Creamos primero el programa fisioterapeutico
                var program = new ProgramaFisioterapeutico()
                {
                    CortoPlazo = request.Program.CortoPlazo,
                    MedianoPlazo = request.Program.MedianoPlazo,
                    LargoPlazo = request.Program.LargoPlazo,
                    TratamientoFisioterapeutico = request.Program.TratamientoFisioterapeutico,
                    Sugerencias = request.Program.Sugerencias,
                    Pronostico = request.Program.Pronostico
                };
                
                await _context.ProgramaFisioterapeuticos.AddAsync(program);
                await _context.SaveChangesAsync();

                // Creamos el mapa corporal
                var mapa = new MapaCorporal()
                {
                    Valor = request.Map.valores,
                    RangoDolor = request.Map.RangoDolor,
                    Nota = request.Map.Nota,
                };
                
                await _context.MapaCorporals.AddAsync(mapa);
                await _context.SaveChangesAsync();

                
                /* Creamos el diagnostico */
                var diagnostico = new Domain.Entities.Diagnostico()
                {
                    Descripcion = request.Diagnostic.Descripcion,
                    Refiere = request.Diagnostic.Refiere,
                    Categoria = request.Diagnostic.Categoria,
                    DiagnosticoPrevio = request.Diagnostic.DiagnosticoPrevio,
                    TerapeuticaEmpleada = request.Diagnostic.TerapeuticaEmpleada,
                    DiagnosticoFuncional = request.Diagnostic.DiagnosticoFuncional,
                    PadecimientoActual = request.Diagnostic.PadecimientoActual,
                    Inspeccion = request.Diagnostic.Inspeccion,
                    ExploracionFisicaCuadro = request.Diagnostic.ExploracionFisicaDescripcion,
                    EstudiosComplementarios = request.Diagnostic.EstudiosComplementarios,
                    DiagnosticoNosologico = request.Diagnostic.DiagnosticoNosologico,
                    Estatus = true,
                    FechaInicio = FormatDate.DateLocal(),
                    ProgramaFisioterapeuticoId = program.ProgramaFisioterapeuticoId,
                    MapaCorporalId = mapa.MapaCorporalId,
                    ExpedienteId = request.ExpedienteId.HashIdInt()
                };
                
                await _context.Diagnosticos.AddAsync(diagnostico);
                await _context.SaveChangesAsync();
                
                // Creamos la revision
                /*
                 * Esta parte se crea aqui, porque la primera vez se crean los datos de la exploracion fisica ya que se
                 * toma como cita al iniciar el diagnostico
                 */
                
                /* Creamos la exploracion Fisica */
                var exploracion = new ExploracionFisica()
                {
                    Fr = request.Exploration.Fr,
                    Fc = request.Exploration.Fc,
                    Temperatura = request.Exploration.Temperatura,
                    Peso = request.Exploration.Peso,
                    Estatura = request.Exploration.Estatura,
                    Imc = request.Exploration.Imc,
                    IndiceCinturaCadera = request.Exploration.IndiceCinturaCadera,
                    SaturacionOxigeno = request.Exploration.SaturacionOxigeno,
                    PresionArterial = request.Exploration.PresionArterial
                };
                
                await _context.ExploracionFisicas.AddAsync(exploracion);
                await _context.SaveChangesAsync();
                
                var revision = new Revision()
                {
                    Notas = request.Review.Notas,
                    FolioPago = request.Review.FolioPago,
                    Fecha = FormatDate.DateLocal(),
                    Hora =  new TimeSpan(FormatDate.DateLocal().Hour, FormatDate.DateLocal().Minute, 0),
                    ExploracionFisicaId = exploracion.ExploracionFisicaId,
                    DiagnosticoId = diagnostico.DiagnosticoId,
                    ServicioId = request.Review.ServicioId.HashIdInt()
                };
                
                await _context.Revisions.AddAsync(revision);
                await _context.SaveChangesAsync();
                
                //Si todo sale bien commitiar 
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Si ocurre un error, revertir la transacción
                try
                {
                    transaction.Rollback();
                }
                catch (Exception exRollback)
                {
                    Console.WriteLine("Error al revertir la transacción: " + exRollback.Message);
                }
                
                throw new BadRequestException("Error al procesar los datos");
            }
        }
    }
}