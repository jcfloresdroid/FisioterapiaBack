using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz.Validator;
using MediatR;

namespace Core.Features.Fisioterapeutas.command;

public record StatusPaciente() : IRequest
{
    public string PacienteId { get; set; }
};

public class StatusPacienteHandler : IRequestHandler<StatusPaciente>
{
    private readonly FisioContext _context;
    private readonly IPacienteValidator _validator;
    
    public StatusPacienteHandler(FisioContext context, IPacienteValidator validator)
    {
        _context = context;
        _validator = validator;
    }
    
    public async Task Handle(StatusPaciente request, CancellationToken cancellationToken)
    {
        // Validaciones
        await _validator.estadoPatient(request);
        
        var paciente = await _context.Pacientes
            .FindAsync(request.PacienteId.HashIdInt());
        
        if (paciente == null)
            throw new NotFoundException("Paciente no encontrado");
        
        paciente.Status = !paciente.Status;

        _context.Pacientes.Update(paciente);
        await _context.SaveChangesAsync();
    }
}