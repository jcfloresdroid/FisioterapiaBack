using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Infraestructure.Persistance;
using MediatR;

namespace Core.Features.Catalogos.command;

public class PostFlujoVaginal : IRequest
{
    public string Descripcion { get; set; }
}

public class PostFlujoVaginalHandler : IRequestHandler<PostFlujoVaginal>
{
    private readonly FisioContext _context;
    
    public PostFlujoVaginalHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task Handle(PostFlujoVaginal request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(request.Descripcion))
            throw new BadRequestException(Message.CAT_0007);
        
        var flujo = new Cat_FlujoVaginal()
        {
            Descripcion = request.Descripcion,
            Status = true
        };
        
        await _context.FlujoVaginals.AddAsync(flujo);
        await _context.SaveChangesAsync();
    }
}