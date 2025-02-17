using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Calendario;

public record ObtenerDatosFecha : IRequest<ObtenerDatosFechaResponse>
{
    public DateTime Fecha { get; init; }
}

public class ObtenerDatosFechaHandler : IRequestHandler<ObtenerDatosFecha, ObtenerDatosFechaResponse>
{
    private readonly FisioContext _context;
    
    public ObtenerDatosFechaHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task<ObtenerDatosFechaResponse> Handle(ObtenerDatosFecha request, CancellationToken cancellationToken)
    {
        var pacientes = await _context.Pacientes
            .Include(x => x.Expediente)
            .Where(p => p.FechaRegistro.Date == request.Fecha.Date)
            .Select(p => new PacientesCalendario()
            {
                PacienteId = p.PacienteId.HashId(),
                Nombre = $"{p.Nombre} {p.Apellido}",
                Edad = FormatDate.DateToYear(p.Edad),
                Sexo = p.Sexo ? "Hombre" : "Mujer",
                Telefono = p.Telefono,
                Foto = p.Foto,
                Status = p.Status,
                Verificado = p.Expediente != null
            })
            .ToListAsync(cancellationToken);
        
        var citas = await _context.Citas
            .Include(x => x.Paciente)
            .Where(p => p.Fecha.Date == request.Fecha.Date)
            .Select(p => new CitaCalendario()
            {
                CitasId = p.CitasId.HashId(),
                Paciente = $"{p.Paciente.Nombre} {p.Paciente.Apellido}",
                Fecha = p.Fecha,
                Hora = p.Hora.ToString(),
                Telefono = p.Paciente.Telefono,
                Foto = p.Paciente.Foto,
                Status = p.Status
                
            })
            .ToListAsync(cancellationToken);
    
        var response = new ObtenerDatosFechaResponse()
        {
            Citas = citas,
            Pacientes = pacientes
        };
        
        return response;
    }
}

public record ObtenerDatosFechaResponse
{
    public IEnumerable<CitaCalendario> Citas { get; set; }
    public IEnumerable<PacientesCalendario> Pacientes { get; set; }
}

public record PacientesCalendario
{
    public string PacienteId { get; set; }
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public string Sexo { get; set; }
    public string Telefono { get; set; }
    public byte[] Foto { get; set; }
    public bool Status { get; set; }
    public bool Verificado { get; set; }
}

public record CitaCalendario
{
    public string CitasId { get; set; }
    public string Paciente { get; set; }
    public DateTime Fecha { get; set; }
    public string Hora { get; set; }
    public string Telefono { get; set; }
    public byte[] Foto { get; set; }
    public int Status { get; set; }
}