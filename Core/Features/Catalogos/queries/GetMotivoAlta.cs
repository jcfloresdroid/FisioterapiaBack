using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Catalogos.queries;

public class GetMotivoAlta : IRequest<List<GetMotivoAltaResponse>>
{
    public bool OnlyActive { get; set; }
}

public class GetMotivoAltaHandler : IRequestHandler<GetMotivoAlta, List<GetMotivoAltaResponse>>
{
    private readonly FisioContext _context;

    public GetMotivoAltaHandler(FisioContext context)
    {
        _context = context;
    }

    public async Task<List<GetMotivoAltaResponse>> Handle(GetMotivoAlta request, CancellationToken cancellationToken)
    {
        var motivo = await _context.MotivoAltas
            .Where(x => !request.OnlyActive || x.Status) //Si solo quiero los activos o todos
            .Select(x => new GetMotivoAltaResponse
            {
                MotivoAltaId = x.MotivoAltaId.HashId(),
                Descripcion = x.Descripcion,
                Status = x.Status
            })
            .ToListAsync(cancellationToken);

        return motivo;
    }
}

public record GetMotivoAltaResponse
{
    public string MotivoAltaId { get; set; }
    public string Descripcion { get; set; }
    public bool Status { get; set; }
}