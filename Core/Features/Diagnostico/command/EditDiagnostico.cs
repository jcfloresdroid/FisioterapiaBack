using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using Core.Services.Interfaz.Validator;
using MediatR;

namespace Core.Features.Diagnostico.command;

public record EditDiagnostico : IRequest
{
    public string DiagnosticoId { get; set; }
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
    
    public string CortoPlazo { get; set; }
    public string MedianoPlazo { get; set; }
    public string LargoPlazo { get; set; }
    public string TratamientoFisioterapeutico { get; set; }
    public string Sugerencias { get; set; }
    public string Pronostico { get; set; }
}

public class EditarDiagnosticoCommand : IRequestHandler<EditDiagnostico>
{
    private readonly FisioContext _context;
    private readonly IExistResource _existResource;
    private readonly IDiagnosticoValidator _validator;

    public EditarDiagnosticoCommand(FisioContext context, IExistResource existResource, IDiagnosticoValidator validator)
    {
        _context = context;
        _validator = validator;
        _existResource = existResource;
    }
    
    public async Task Handle(EditDiagnostico request, CancellationToken cancellationToken)
    {
        await _existResource.ExistDiagnostico(request.DiagnosticoId);
        await _validator.ModificarDiagnostico(request);
        
        var diagnostico = await _context.Diagnosticos.FindAsync(request.DiagnosticoId.HashIdInt());
        
        diagnostico.Descripcion = request.Descripcion;
        diagnostico.Refiere = request.Refiere;
        diagnostico.Categoria = request.Categoria;
        diagnostico.DiagnosticoPrevio = request.DiagnosticoPrevio;
        diagnostico.TerapeuticaEmpleada = request.TerapeuticaEmpleada;
        diagnostico.DiagnosticoFuncional = request.DiagnosticoFuncional;
        diagnostico.PadecimientoActual = request.PadecimientoActual;
        diagnostico.Inspeccion = request.Inspeccion;
        diagnostico.ExploracionFisicaCuadro = request.ExploracionFisicaDescripcion;
        diagnostico.EstudiosComplementarios = request.EstudiosComplementarios;
        diagnostico.DiagnosticoNosologico = request.DiagnosticoNosologico;
        
        var programa = await _context.ProgramaFisioterapeuticos.FindAsync(diagnostico.ProgramaFisioterapeuticoId);
        
        programa.CortoPlazo = request.CortoPlazo;
        programa.MedianoPlazo = request.MedianoPlazo;
        programa.LargoPlazo = request.LargoPlazo;
        programa.TratamientoFisioterapeutico = request.TratamientoFisioterapeutico;
        programa.Sugerencias = request.Sugerencias;
        programa.Pronostico = request.Pronostico;
        
        _context.Diagnosticos.Update(diagnostico);
        _context.ProgramaFisioterapeuticos.Update(programa);
        await _context.SaveChangesAsync(cancellationToken);
    }
}