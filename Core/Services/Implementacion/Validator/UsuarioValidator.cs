using Application.Core.Domain.Exceptions;
using Core.Features.Usuario.command;
using Core.Services.Interfaz.Validator;
using Core.Validator.Usuario;

namespace Core.Services.Implementacion.Validator;

public class UsuarioValidator : IUsuarioValidator
{
    private readonly UserValidator _validator;
    private readonly UserMValidator _validatorM;
    private readonly FotoMValidator _validatorF;
    
    public UsuarioValidator(UserValidator validator, UserMValidator validatorM, FotoMValidator validatorF)
    {
        _validator = validator;
        _validatorM = validatorM;
        _validatorF = validatorF;
    }
    
    public async Task CreateUser(CreateUser createUser)
    {
        var userValidator = await _validator.ValidateAsync(createUser);

        if (!userValidator.IsValid)
            throw new ValidationException(userValidator.Errors);
    }

    public async Task ModificarUsuario(ModificarUsuario modificarUsuario)
    {
        var userValidator = await _validatorM.ValidateAsync(modificarUsuario);

        if (!userValidator.IsValid)
            throw new ValidationException(userValidator.Errors);
    }

    public async Task ModificarFoto(ModificarFoto modificarFoto)
    {
        var userValidator = await _validatorF.ValidateAsync(modificarFoto);

        if (!userValidator.IsValid)
            throw new ValidationException(userValidator.Errors);
    }
}