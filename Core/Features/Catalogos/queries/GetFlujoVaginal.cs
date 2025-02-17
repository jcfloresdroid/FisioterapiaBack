using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Catalogos.queries;

public class GetFlujoVaginal : IRequest<List<GetFlujoVaginalResponse>>
{
    public bool OnlyActive { get; set; }
}

public class GetFlujoVaginalHandler : IRequestHandler<GetFlujoVaginal, List<GetFlujoVaginalResponse>>
{
    private readonly FisioContext _context;

    public GetFlujoVaginalHandler(FisioContext context)
    {
        _context = context;
    }

    public async Task<List<GetFlujoVaginalResponse>> Handle(GetFlujoVaginal request, CancellationToken cancellationToken)
    {
        var flujo = await _context.FlujoVaginals
            .Where(x => !request.OnlyActive || x.Status) //Si solo quiero los activos o todos
            .Select(x => new GetFlujoVaginalResponse
            {
                FlujoVaginalId = x.FlujoVaginalId.HashId(),
                Descripcion = x.Descripcion,
                Status = x.Status
            })
            .ToListAsync(cancellationToken);

        return flujo;
    }
}

public record GetFlujoVaginalResponse
{
    public string FlujoVaginalId { get; set; }
    public string Descripcion { get; set; }
    public bool Status { get; set; }
}