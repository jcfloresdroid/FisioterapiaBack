using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Catalogos.queries;

public class GetServicios : IRequest<List<GetServiciosResponse>>
{
    public bool OnlyActive { get; set; }
}

public class GetServiciosHandler : IRequestHandler<GetServicios, List<GetServiciosResponse>>
{
    private readonly FisioContext _context;

    public GetServiciosHandler(FisioContext context)
    {
        _context = context;
    }

    public async Task<List<GetServiciosResponse>> Handle(GetServicios request, CancellationToken cancellationToken)
    {
        var servicios = await _context.Servicios
            .Where(x => !request.OnlyActive || x.Status) //Si solo quiero los activos o todos
            .Select(x => new GetServiciosResponse
            {
                ServicioId = x.ServiciosId.HashId(),
                Descripcion = x.Descripcion,
                Status = x.Status
            })
            .ToListAsync(cancellationToken);

        return servicios;
    }
}

public record GetServiciosResponse
{
    public string ServicioId { get; set; }
    public string Descripcion { get; set; }
    public bool Status { get; set; }
}