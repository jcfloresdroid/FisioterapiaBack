using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Infraestructure.Persistance;
using MediatR;

namespace Core.Features.Catalogos.command;

public class PostServicios : IRequest
{
    public string Descripcion { get; set; }
}

public class PostServiciosHandler : IRequestHandler<PostServicios>
{
    private readonly FisioContext _context;
    
    public PostServiciosHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task Handle(PostServicios request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(request.Descripcion))
            throw new BadRequestException(Message.CAT_0007);
        
        var servicios = new Cat_Servicios()
        {
            Descripcion = request.Descripcion,
            Status = true
        };
        
        await _context.Servicios.AddAsync(servicios);
        await _context.SaveChangesAsync();
    }
}