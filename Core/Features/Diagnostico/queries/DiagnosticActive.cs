using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Diagnostico.queries;

public record DiagnosticActive : IRequest<DiagnosticActiveResponse>
{
    public string ExpedienteId { get; set; }
}

public class DiagnosticoActiveHandler : IRequestHandler<DiagnosticActive, DiagnosticActiveResponse>
{
    private readonly FisioContext _context;
    
    public DiagnosticoActiveHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task<DiagnosticActiveResponse> Handle(DiagnosticActive request, CancellationToken cancellationToken)
    {
        var diagnostic = await _context.Diagnosticos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ExpedienteId == request.ExpedienteId.HashIdInt() && x.Estatus == true);
        
        if (diagnostic == null) {
            var response = new DiagnosticActiveResponse() {
                EnCurso = false
            };

            return response;
        } else {
            var response = new DiagnosticActiveResponse() {
                EnCurso = true,
                DiagnosticoId = diagnostic.DiagnosticoId.HashId()
            };
            
            return response;
        }
    }
}

public record DiagnosticActiveResponse
{
    public bool EnCurso { get; set; }
    public string? DiagnosticoId { get; set; }
}