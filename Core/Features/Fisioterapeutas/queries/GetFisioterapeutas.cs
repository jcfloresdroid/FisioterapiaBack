using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features;

public record GetFisioterapeutas() : IRequest<GetFisioterapeutaResponse>
{
    public int Pagina { get; set; }
    public bool OnlyActive { get; set; } = true;
}

public class GetFisioterapeutaHandler : IRequestHandler<GetFisioterapeutas, GetFisioterapeutaResponse>
{
    private readonly FisioContext _context;

    public GetFisioterapeutaHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task<GetFisioterapeutaResponse> Handle(GetFisioterapeutas request, CancellationToken cancellationToken)
    {
        // Obtener el número total de paginas
        var pageFisios = await _context.Fisioterapeuta
            .AsNoTracking()
            .Where(x => !request.OnlyActive || x.Status) //Si solo quiero los activos o todos
            .ToListAsync();
        
        // Calculamos el número de páginas
        int numPage = (int)Math.Ceiling((double)pageFisios.Count / 10);
        
        //Devuelve una lista de 10 fisios
        var fisios = await _context.Fisioterapeuta
            .AsNoTracking()
            .Where(x => !request.OnlyActive || x.Status)
            .OrderBy(x => x.Nombre)
            .Include(x => x.Especialidades)
            .Skip((request.Pagina - 1) * 10)
            .Take(10)
            .Select(x => new FisioDto()
            {
                FisioterapeutaId = x.FisioterapeutaId.HashId(),
                Nombre = x.Nombre,
                CedulaProfesional = x.CedulaProfesional,
                Correo = x.Correo,
                Telefono = x.Telefono,
                Status = x.Status,
                Especialidad = x.Especialidades.Descripcion,
                Foto = x.FotoPerfil
            }).ToListAsync();

        var response = new GetFisioterapeutaResponse()
        {
            NumPaginas = numPage,
            Total = pageFisios.Count,
            Fisioterapeutas = fisios
        };

        return response;
    }
}

public record GetFisioterapeutaResponse{
    public int NumPaginas { get; set; }
    public int Total { get; set; }
    public List<FisioDto> Fisioterapeutas { get; set; }
}

public record FisioDto
{
    public string FisioterapeutaId { get; set; }
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Telefono { get; set; }
    public string CedulaProfesional { get; set; }
    public byte[] Foto { get; set; }
    public bool Status { get; set; }
    public string Especialidad { get; set; }
}