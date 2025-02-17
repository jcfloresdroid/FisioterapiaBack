using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features;

public record UltimosFisios() : IRequest<List<UltimosFisiosResponse>>;

public class UltimosFisiosHandler : IRequestHandler<UltimosFisios, List<UltimosFisiosResponse>>
{
    private readonly FisioContext _context;

    public UltimosFisiosHandler(FisioContext context)
    {
        _context = context;
    }

    public async Task<List<UltimosFisiosResponse>> Handle(UltimosFisios request, CancellationToken cancellationToken)
    {
        var fisios = await _context.Fisioterapeuta
            .AsNoTracking()
            .OrderByDescending(x => x.FechaRegistro)
            .Take(6)
            .Select(x => new UltimosFisiosResponse
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

public record UltimosFisiosResponse()
{
    public string Nombre { get; set; }
    public string Telefono { get; set; }
    public string Correo { get; set; }
    public byte[] FotoPerfil { get; set; }
}