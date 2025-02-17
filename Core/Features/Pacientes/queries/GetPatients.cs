using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Core.Domain.Helpers;

namespace Core.Features.Pacientes.queries;

public record GetPatients : IRequest<GetPatientsResponse>
{
    public int Pagina { get; set; }
    public bool OnlyActive { get; set; }
};

public class GetPatientsHandler : IRequestHandler<GetPatients, GetPatientsResponse>
{
    private readonly FisioContext _context;

    public GetPatientsHandler(FisioContext context)
    {
        _context = context;
    }

    public async Task<GetPatientsResponse> Handle(GetPatients request, CancellationToken cancellationToken)
    {
        // Obtener el número total de paginas
        var totalPatients = await _context.Pacientes
            .AsNoTracking()
            .Where(x => !request.OnlyActive || x.Status) //Si solo quiero los activos o todos
            .ToListAsync();
        
        // Calculamos el número de páginas
        int numPage = (int)Math.Ceiling((double)totalPatients.Count / 10);
        
        //Devuelve una lista de 10 pacientes
        var patients = await _context.Pacientes
            .AsNoTracking()
            .Where(x => !request.OnlyActive || x.Status)
            .Include(x => x.Fisioterapeuta)
            .Include(x => x.Expediente)
            .OrderBy(x => x.Nombre)
            .Skip((request.Pagina - 1) * 10)
            .Take(10)
            .Select(x => new GetPacientesModel()
            {
                PacienteId = x.PacienteId.HashId(), 
                Nombre = $"{x.Nombre} {x.Apellido}",
                Sexo = x.Sexo == true ? "Hombre" : "Mujer",
                Telefono = x.Telefono,
                Fisioterapeuta = x.Fisioterapeuta.Nombre,
                Edad = FormatDate.DateToYear(x.Edad.Date),
                Estatus = x.Status,
                Verificado = x.Expediente != null,
                Foto = x.Foto
            }).ToListAsync();

        // Response
        var response = new GetPatientsResponse()
        {
            numPaginas = numPage,
            total = totalPatients.Count,
            pacientes = patients
        };
        
        return response;
    }
}

public record GetPatientsResponse
{
    public int numPaginas { get; set; }
    public int total { get; set; }
    public List<GetPacientesModel> pacientes { get; set; }
}

public record GetPacientesModel
{
    public string PacienteId { get; set; } 
    public string Nombre { get; set; }
    public string Sexo { get; set; }
    public string Telefono { get; set; }
    public string Fisioterapeuta { get; set; }
    public int Edad { get; set; }
    public bool Estatus { get; set; }
    public bool Verificado { get; set; } //Este son para los que tienen completado el expediente
    public byte[] Foto { get; set; }
}