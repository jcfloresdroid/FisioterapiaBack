using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Catalogos.queries;

public class GetPatologias : IRequest<List<GetPatologiasResponse>>
{
    public bool OnlyActive { get; set; }
}

public class GetPatologiasHandler : IRequestHandler<GetPatologias, List<GetPatologiasResponse>>
{
    private readonly FisioContext _context;

    public GetPatologiasHandler(FisioContext context)
    {
        _context = context;
    }

    public async Task<List<GetPatologiasResponse>> Handle(GetPatologias request, CancellationToken cancellationToken)
    {
        var patologias = await _context.Patologias
            .Where(x => !request.OnlyActive || x.Status) 
            .Select(x => new GetPatologiasResponse
            {
                PatologiasId = x.PatologiasId.HashId(),
                Descripcion = x.Descripcion,
                Status = x.Status
            })
            .ToListAsync(cancellationToken);

        return patologias;
    }
}

public record GetPatologiasResponse
{
    public string PatologiasId { get; set; }
    public string Descripcion { get; set; }
    public bool Status { get; set; }
}