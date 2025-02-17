using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Infraestructure.Persistance;
using MediatR;

namespace Core.Features.Catalogos.command;

public class PostEspecialidades : IRequest
{
    public string Descripcion { get; set; }
}

public class PostEspecialidadesHandler : IRequestHandler<PostEspecialidades>
{
    private readonly FisioContext _context;
    
    public PostEspecialidadesHandler(FisioContext context)
    {
        _context = context;
    }
    
    public async Task Handle(PostEspecialidades request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(request.Descripcion))
            throw new BadRequestException(Message.CAT_0007);
        
        var especialidades = new Cat_Especialidades()
        {
            Descripcion = request.Descripcion,
            Status = true
        };
        
        await _context.Especialidades.AddAsync(especialidades);
        await _context.SaveChangesAsync();
    }
}