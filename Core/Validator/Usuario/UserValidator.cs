using Core.Features.Usuario.command;
using FluentValidation;

namespace Core.Validator.Usuario;

public class UserValidator : AbstractValidator<CreateUser>
{
    public UserValidator()
    {
        RuleFor(x => x.Username)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.USER_00002);
        
        RuleFor(x => x.Contraseña)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.USER_00003);
        
        RuleFor(x => x.EspecialidadId)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Message.USER_00004);
    }
}