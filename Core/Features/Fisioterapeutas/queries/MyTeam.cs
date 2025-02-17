using Core.Infraestructure.Persistance;
using iTextSharp.text;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features;

public record MyTeam() : IRequest<List<MyTeamResponse>>;

public class MyTeamHandler : IRequestHandler<MyTeam, List<MyTeamResponse>>
{
    private readonly FisioContext _context;

    public MyTeamHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task<List<MyTeamResponse>> Handle(MyTeam request, CancellationToken cancellationToken)
    {
        var fisios = await _context.Fisioterapeuta
            .AsNoTracking()
            .Where(x => x.Status)
            .OrderByDescending(x => x.FechaRegistro)
            .Select(x => new MyTeamResponse
            {
                Nombre = x.Nombre,
                Telefono = x.Telefono,
                Correo = x.Correo,
                FotoPerfil = x.FotoPerfil
            })
            .ToListAsync();

        return fisios;
    }
}

public record MyTeamResponse
{
    public string Nombre { get; set; }
    public string Telefono { get; set; }
    public string Correo { get; set; }
    public byte[] FotoPerfil { get; set; }
}