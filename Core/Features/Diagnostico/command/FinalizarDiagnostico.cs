using System.ComponentModel.DataAnnotations;
using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using Core.Services.Interfaz.Validator;
using MediatR;

namespace Core.Features.Diagnostico.command;

public record FinalizarDiagnostico : IRequest
{
    public string DiagnosticId { get; set; }
    public string DiagnosticoInicial { get; set; }
    public string DiagnosticoFinal { get; set; }
    public string FrecuenciaTratamiento { get; set; }
    public string MotivoAltaId { get; set; }
}

public class FinalizarDiagnosticoHandler : IRequestHandler<FinalizarDiagnostico>
{
    private readonly FisioContext _context;
    private readonly IDiagnosticoValidator _validator;
    private readonly IExistResource _existResource;
    
    public FinalizarDiagnosticoHandler(FisioContext context, IDiagnosticoValidator validator, IExistResource existResource)
    {
        _context = context;
        _validator = validator;
        _existResource = existResource;
    }
    
    public async Task Handle(FinalizarDiagnostico request, CancellationToken cancellationToken)
    {
        // Valicaciones
        await _validator.FinDiagnostico(request);
        
        // Existencia de recursos
        await _existResource.ExistMotivoAlta(request.MotivoAltaId);
        
        var diagnostic = await _context.Diagnosticos
            .FindAsync(request.DiagnosticId.HashIdInt())
            ?? throw new NotFoundException("diagnostico no encontrado");
        
        diagnostic.DiagnosticoInicial = request.DiagnosticoInicial;
        diagnostic.DiagnosticoFinal = request.DiagnosticoFinal;
        diagnostic.FrecuenciaTratamiento = request.FrecuenciaTratamiento;
        diagnostic.Estatus = false;
        diagnostic.FechaAlta = FormatDate.DateLocal().Date;
        diagnostic.MotivoAltaId = request.MotivoAltaId.HashIdInt();
        
        _context.Diagnosticos.Update(diagnostic);
        await _context.SaveChangesAsync();
    }
}