using Core.Features.Usuario.command;

namespace Core.Services.Interfaz.Validator;

public interface IUsuarioValidator
{
    Task CreateUser(CreateUser createUser);
    
    Task ModificarUsuario(ModificarUsuario modificarUsuario);
    
    Task ModificarFoto(ModificarFoto modificarFoto);
}