using Core.Domain.Exceptions;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using MediatR;

namespace Core.Features.Usuario.queries;

public record ViewPicture : IRequest<ViewPictureResponse>{}

public class ViewPictureHandler : IRequestHandler<ViewPicture, ViewPictureResponse>
{
    private readonly IAuthorization _authorization;
    private readonly FisioContext _context;

    public ViewPictureHandler(IAuthorization authorization, FisioContext context)
    {
        _authorization = authorization;
        _context = context;
    }

    public async Task<ViewPictureResponse> Handle(ViewPicture request, CancellationToken cancellationToken)
    {
        var user = await _context.Usuarios
            .FindAsync(_authorization.UsuarioActual())
            ?? throw new NotFoundException(Message.USER_00001);
        
        return await Task.FromResult(new ViewPictureResponse()
        {
            Foto = user.FotoPerfil,
            Username = user.Username
        });
    }
}

public record ViewPictureResponse
{
    public byte[] Foto { get; set; }
    public string Username { get; set; }
}