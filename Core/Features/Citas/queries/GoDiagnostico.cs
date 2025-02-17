using Core.Domain.Enum;
using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Citas.queries;

public record GoDiagnostico : IRequest<GoDiagnosticoResponse>
{
    public string PacienteId { get; set; }
    public string CitaId { get; set; }
}

public class GoDiagnosticoHandler : IRequestHandler<GoDiagnostico, GoDiagnosticoResponse>
{
    private readonly FisioContext _context;

    public GoDiagnosticoHandler(FisioContext context)
    {
        _context = context;
    }

    public async Task<GoDiagnosticoResponse> Handle(GoDiagnostico request, CancellationToken cancellationToken)
    {
        var expediente = await _context.Expedientes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PacienteId == request.PacienteId.HashIdInt())
            ?? throw new NotFoundException(Message.PACI_0016);

        var diagnostico = await _context.Diagnosticos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ExpedienteId == expediente.ExpedienteId && x.Estatus);
        
        // Confirmamos la cita como asistencia
        var cita = await _context.Citas
            .FindAsync(request.CitaId.HashIdInt())
            ?? throw new NotFoundException(Message.CITA_0004);

        cita.Status = (int)EstadoCita.Concluida;
        
        // Guardamos los cambios
        await _context.SaveChangesAsync();
        
        var response = new GoDiagnosticoResponse()
        {
            DiagnosticoId = diagnostico?.DiagnosticoId.HashId()
        };

        return response;
    }
}

public record GoDiagnosticoResponse
{
    public string? DiagnosticoId { get; set; }
}