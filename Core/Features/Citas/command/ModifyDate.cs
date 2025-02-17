using Core.Domain.Enum;
using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;

namespace Core.Features.Citas.command;

public record ModifyDate : IRequest
{
    public string CitaId { get; set; }
    public bool? Cancelar { get; set; }
    public DateTime? Fecha { get; set; }
    public TimeSpan? Hora { get; set; }
    public string? Motivo { get; set; }
};

public class ModifyDateHandler : IRequestHandler<ModifyDate>
{
    private readonly FisioContext _context;

    public ModifyDateHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task Handle(ModifyDate request, CancellationToken cancellationToken)
    {
        if(request.CitaId == null)
            throw new BadRequestException(Message.CITA_0004);
        
        var date = await _context.Citas
            .FindAsync(request.CitaId.HashIdInt())
            ?? throw new NotFoundException(Message.CITA_0005);

        // Actualizaremos solo los datos no nulos
        if (request.Cancelar == true)
            date.Status = (int)EstadoCita.Cancelada;

        if (request.Fecha.HasValue)
            date.Fecha = request.Fecha.Value;

        if (request.Hora.HasValue)
            date.Hora = request.Hora.Value;

        date.Motivo = request.Motivo ?? date.Motivo;
        
        // Guardamos los cambios
        await _context.SaveChangesAsync(cancellationToken);
    }
}