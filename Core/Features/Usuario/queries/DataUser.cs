using Core.Domain.Exceptions;
using Core.Domain.Helpers;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using MediatR;

namespace Core.Features.Usuario.queries;

public record DataUser : IRequest<DataUserResponse> { }

public class DataUserHandler : IRequestHandler<DataUser, DataUserResponse>
{
    private readonly FisioContext _context;
    private readonly IAuthorization _authorization;
    
    public DataUserHandler(FisioContext context, IAuthorization authorization)
    {
        _context = context;
        _authorization = authorization;
    }
    
    public async Task<DataUserResponse> Handle(DataUser request, CancellationToken cancellationToken)
    {
        var user = await _context.Usuarios
            .FindAsync(_authorization.UsuarioActual())
            ?? throw new NotFoundException(Message.USER_00001);
        
        return await Task.FromResult(new DataUserResponse()
        {
            UsuarioId = user.UsuarioId,
            Username = user.Username,
            Correo = user.Correo,
            Telefono = user.Telefono,
            EspecialidadId = user.EspecialidadId.Value.HashId(),
            Foto = user.FotoPerfil
        });
    }
}

public record DataUserResponse
{
    public int UsuarioId { get; set; }
    public string Username { get; set; }
    public string Correo { get; set; }
    public string Telefono { get; set; }
    public string EspecialidadId { get; set; }
    public byte[] Foto { get; set; }
}