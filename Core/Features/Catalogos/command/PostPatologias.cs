using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Infraestructure.Persistance;
using MediatR;

namespace Core.Features.Catalogos.command;

public class PostPatologias : IRequest
{
    public string Descripcion { get; set; }
}

public class PostPatologiasHandler : IRequestHandler<PostPatologias>
{
    private readonly FisioContext _context;
    
    public PostPatologiasHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task Handle(PostPatologias request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(request.Descripcion))
            throw new BadRequestException(Message.CAT_0007);
        
        var patologias = new Cat_Patologias()
        {
            Descripcion = request.Descripcion,
            Status = true
        };
        
        await _context.Patologias.AddAsync(patologias);
        await _context.SaveChangesAsync();
    }
}