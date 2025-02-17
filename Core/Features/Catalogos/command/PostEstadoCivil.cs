using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Infraestructure.Persistance;
using MediatR;

namespace Core.Features.Catalogos.command;

public class PostEstadoCivil : IRequest
{
    public string Descripcion { get; set; }
}

public class PostEstadoCivilHandler : IRequestHandler<PostEstadoCivil>
{
    private readonly FisioContext _context;
    
    public PostEstadoCivilHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task Handle(PostEstadoCivil request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(request.Descripcion))
            throw new BadRequestException(Message.CAT_0007);
        
        var estadoCivil = new Cat_EstadoCivil()
        {
            Descripcion = request.Descripcion,
            Status = true
        };
        
        await _context.EstadoCivils.AddAsync(estadoCivil);
        await _context.SaveChangesAsync();
    }
}