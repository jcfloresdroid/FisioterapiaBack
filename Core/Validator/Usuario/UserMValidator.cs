using Core.Features.Usuario.command;
using FluentValidation;

namespace Core.Validator.Usuario;

public class UserMValidator : AbstractValidator<ModificarUsuario>
{
    public UserMValidator()
    {
        RuleFor(x => x.Correo)
            .EmailAddress()
            .WithMessage(Message.USER_00005);
        
        RuleFor(x => x.Telefono)
            .Matches(@"^\d{10}$")
            .WithMessage(Message.USER_00006);
    }
}