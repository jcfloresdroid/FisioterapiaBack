using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Pacientes.queries;

public record LastPatients : IRequest<List<LastPatientsResponse>>;

public class LastPatientsHandler : IRequestHandler<LastPatients, List<LastPatientsResponse>>
{
    private readonly FisioContext _context;

    public LastPatientsHandler(FisioContext context)
    {
        _context = context;
    }

    public async Task<List<LastPatientsResponse>> Handle(LastPatients request, CancellationToken cancellationToken)
    {
        var patients = await _context.Pacientes
            .AsNoTracking()
            .Where(x => x.Status == true)
            .OrderByDescending(x => x.PacienteId)
            .Take(7)
            .Select(x => new LastPatientsResponse()
            {
                Nombre = $"{x.Nombre} {x.Apellido}",
                Telefono = x.Telefono,
                Edad = FormatDate.DateToYear(x.Edad.Date),
                Sexo = x.Sexo ? "Hombre" : "Mujer"
            }).ToListAsync();

        return patients;
    }
}

public record LastPatientsResponse
{
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public string Sexo { get; set; }
    public string Telefono { get; set; }
}