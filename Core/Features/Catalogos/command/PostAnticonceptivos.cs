using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Infraestructure.Persistance;
using MediatR;

namespace Core.Features.Catalogos.command;

public record PostAnticonceptivos : IRequest
{
    public string Descripcion { get; set; }
}

public class PostAnticonceptivosHandler : IRequestHandler<PostAnticonceptivos>
{
    private readonly FisioContext _context;
    
    public PostAnticonceptivosHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task Handle(PostAnticonceptivos request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(request.Descripcion))
            throw new BadRequestException(Message.CAT_0007);
        
        var anticonceptivo = new Cat_TipoAnticonceptivo()
        {
            Descripcion = request.Descripcion,
            Status = true
        };
        
        await _context.TipoAnticonceptivos.AddAsync(anticonceptivo);
        await _context.SaveChangesAsync();
    }
}