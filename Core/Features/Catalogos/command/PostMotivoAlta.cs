using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Infraestructure.Persistance;
using MediatR;

namespace Core.Features.Catalogos.command;

public class PostMotivoAlta : IRequest
{
    public string Descripcion { get; set; }
}

public class PostMotivoAltaHandler : IRequestHandler<PostMotivoAlta>
{
    private readonly FisioContext _context;
    
    public PostMotivoAltaHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task Handle(PostMotivoAlta request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(request.Descripcion))
            throw new BadRequestException(Message.CAT_0007);
        
        var alta = new Cat_MotivoAlta()
        {
            Descripcion = request.Descripcion,
            Status = true
        };
        
        await _context.MotivoAltas.AddAsync(alta);
        await _context.SaveChangesAsync();
    }
}