using Core.Domain.Entities;
using Core.Domain.Enum;
using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz.Validator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Citas.command;

public record PostDate : IRequest
{
    public string PacienteId { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan Hora { get; set; }
    public string Motivo { get; set; }
}

public class PostDateHandler : IRequestHandler<PostDate>
{
    private readonly FisioContext _context;
    private readonly ICitasValidator _validator;
    
    public PostDateHandler(FisioContext context, ICitasValidator validator)
    {
        _context = context;
        _validator = validator;
    }
    
    public async Task Handle(PostDate request, CancellationToken cancellationToken)
    {
        // Validaciones
        await _validator.AgregarCita(request);
        
        var patient = await _context.Pacientes
            .AsNoTracking()
            .Where(x => x.Status)
            .Include(x => x.Fisioterapeuta)
            .FirstOrDefaultAsync(x => x.PacienteId == request.PacienteId.HashIdInt())
            ?? throw new NotFoundException(Message.PACI_0017);
        
        if(patient.Fisioterapeuta.Status == false)
            throw new BadRequestException(Message.FIS_0009);
        
        //Validar si la cita se puede llevar acabo
        await ValidateDate(request.Fecha, request.Hora, patient);
        
        var date = new Cita()
        {
            PacienteId = request.PacienteId.HashIdInt(),
            FisioterapeutaId = patient.Fisioterapeuta.FisioterapeutaId,
            Fecha = request.Fecha,
            Hora = request.Hora,
            Motivo = request.Motivo,
            Status = (int)EstadoCita.Pendiente
        };

        await _context.Citas.AddAsync(date);
        await _context.SaveChangesAsync();
    }
    
    public async Task ValidateDate(DateTime Fecha, TimeSpan Hora, Paciente paciente)
    {
        //Validamos que la fecha no este pasada
        if (Fecha.Date < FormatDate.DateLocal().Date)
            throw new BadRequestException(Message.GRAL_0001);
        
        //Validamos que la hora no este pasada
        if (Fecha.Date == FormatDate.DateLocal().Date && Hora < FormatHour.LessHour(FormatDate.DateLocal().TimeOfDay))
            throw new BadRequestException(Message.GRAL_0002);
        
        //Validamos que el usuario no pueda agendar mas de una cita al dia
        var citas = await _context.Citas
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PacienteId == paciente.PacienteId && x.Fecha.Date == Fecha.Date);
        
        if (citas != null)
            throw new BadRequestException(Message.GRAL_0003);
        
        //Validamos que el usuario no pueda agendar mas de 3 citas a la semana
        var citasSemana = await _context.Citas
            .AsNoTracking()
            .Where(x => x.Fecha.Date >= FormatDate.StartOfWeek().Date && x.Fecha.Date <= FormatDate.EndOfWeek().Date && x.PacienteId == paciente.PacienteId)
            .ToListAsync();
        
        if(citasSemana.Count >= 3)
            throw new BadRequestException(Message.GRAL_0004);
        
        /* ----------------------------------------- general -------------------------------------------------------- */
        
        var allCitas = await _context.Citas
            .AsNoTracking()
            .Include(x => x.Paciente)
            .ThenInclude(x => x.Fisioterapeuta)
            .Where(x => x.Fecha.Date == Fecha.Date && x.Hora < FormatHour.MoreHours(Hora) && x.Hora > FormatHour.LessHour(Hora))
            .ToListAsync();
        
        //No se puede agendar mas de 3 citas a la misma hora
        if(allCitas.Count >= 3)
            throw new BadRequestException(Message.GRAL_0005);

        //Validamos que el fisioterapeuta no tenga citas en la misma hora
        foreach (var fisios in allCitas)
        {
            if (fisios.Paciente.Fisioterapeuta.FisioterapeutaId == paciente.Fisioterapeuta.FisioterapeutaId)
                throw new BadRequestException(Message.GRAL_0006);
        }
    }
}